using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;// for inherit class from agent
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
//to access to the BehaviourParameter.
//At runtime, this component generates the agent's policy objects according to the settings you specified in the Editor.

public enum GameResultType { Win, Lose, Draw };
public class MinerAgent : Agent
{
	GameGroup gameGroup;

	[HideInInspector]
	public Rigidbody rB;
	Vector3 originalPos;
	Quaternion originalRota;
	[ColorUsage(true, true)]
	Color originalColor;//color will change when the player is dashing
	[SerializeField]
	[ColorUsage(true,true)]
	Color dashColor;//define the color when the cueb is dashing
	[SerializeField]
	Renderer cubeRenderer;// define the renderer of the cube
	[SerializeField]
	float rotateSpeed = 3;
	[SerializeField]
	float speed = 100;
	[SerializeField]
	float maxSpeed = 3;
	[SerializeField]
	float maxDashSpeed = 6;


	//dashing pis taking effect
	bool dashing = false;
	float dashingTimer = 0;
	float dashingTimerTotal = .3f;//how long the dashing would be

	

	//dashing is ready to use
	bool dashCDReady = true;
	float dashCDTimer = 0;
	float dashCDTimerTotal = 2;

	int teamID = -1;
	int teamID_Opponent = -1;

	public override void Initialize()
	{
		rB = GetComponent<Rigidbody>();
		gameGroup = GetComponentInParent<GameGroup>();
		originalPos = transform.localPosition;
		originalRota = transform.localRotation;
		originalColor = cubeRenderer.material.GetColor("_Color");
		teamID = GetComponent<BehaviorParameters>().TeamId;

		if (teamID == 1)
		{
			teamID_Opponent = 0;
		}
		else
		{
			teamID_Opponent = 1;
		}
	}
	public override void OnEpisodeBegin()
	{
		transform.localPosition = originalPos;
		transform.localRotation = originalRota;
		cubeRenderer.material.SetColor("_Color", originalColor);
		rB.velocity = Vector3.zero;

	}
	public override void CollectObservations(VectorSensor sensor)
	{
		sensor.AddObservation(rB.velocity);

		sensor.AddObservation(dashing);
		sensor.AddObservation(dashingTimer / dashingTimerTotal);
		sensor.AddObservation(dashCDTimer / dashCDTimerTotal);
	}

	//set how and when the ai can recieve actions from specific action branch
	public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
	{
		actionMask.SetActionEnabled(2, 1, dashCDReady);
	}
	public override void OnActionReceived(ActionBuffers actions)
	{
		//Actions: 0=Do nothing 1=Up/Right 2=Down/Left
		float rotateValue = 0;
		float forwardValue = 0;
		var discreteActions = actions.DiscreteActions;

		int rotateAction = discreteActions[0];
		int forwardAction = discreteActions[1];
		int dashAction = discreteActions[2];

		switch(rotateAction)
		{
			case 1:
				rotateValue = 1;
				break;
			case 2:
				rotateValue = -1;
				break;
		}

		switch (forwardAction)
		{
			case 1:
				forwardValue = 1;
				break;
			case 2:
				forwardValue = -1;
				break;
		}

		Vector3 force = transform.forward * forwardValue * (speed + dashAction * 2);

		if (dashAction == 1 && forwardAction == 1)
		{
			cubeRenderer.material.SetColor("_Color", dashColor);
			dashCDReady = false;
			dashing = true;
			dashingTimer = dashingTimerTotal;
			AddReward(-.01f);
		}

		if (dashing)
		{
			force = force * 1.5f;
		}

		rB.AddForce(force, ForceMode.VelocityChange);
		transform.Rotate(transform.up * rotateValue, rotateSpeed);

	}

	private void FixedUpdate()
	{
		if (!dashing)//agent is not dashing
		{
			if (rB.velocity.magnitude > maxSpeed)//constraint the max speed
			{
				rB.velocity = rB.velocity.normalized * maxSpeed;
			}

			if (!dashCDReady)
			{
				if (dashCDTimer > 0)//count down the dashing cd
				{
					dashCDTimer -= Time.fixedDeltaTime;
				}
				else
				{
					dashCDTimer = 0;
					dashCDReady = true;//agent is ready to dash again
				}
			}
		}
		else
		{
			if (rB.velocity.magnitude > maxDashSpeed)
			{
				rB.velocity = rB.velocity.normalized * maxDashSpeed;
			}

			if(dashingTimer > 0)// agent is dashing now
			{
				dashingTimer -= Time.fixedDeltaTime;
			}
			else//dashing is over
			{
				dashingTimer = 0;
				dashCDTimer = dashCDTimerTotal;
				dashing = false;
				cubeRenderer.material.SetColor("_Color", originalColor);
			}
		}
	}
	public override void Heuristic(in ActionBuffers actionsOut)
	{

		float rotateValue = Input.GetAxis("Horizontal");
		float forwardValue = Input.GetAxis("Vertical");

		int rotateAction = 0;
		int forwardAction = 0;

		if (rotateValue > 0.01f)
		{
			rotateAction = 1;
		}
		else if (rotateValue < -0.01f)
		{
			rotateAction = 2;
		}

		if (forwardValue > 0.01f)
		{
			forwardAction = 1;
		}
		else if (forwardValue < -0.01f)
		{
			forwardAction = 2;
		}

		var discreteActions = actionsOut.DiscreteActions;
		discreteActions[0] = rotateAction;
		discreteActions[1] = forwardAction;

		if (Input.GetKey(KeyCode.Space) && dashCDReady)
		{
			discreteActions[2] = 1;
		}
	}


	private void OnTriggerEnter(Collider other)
	{
		if (!other.tag.Contains("Gold")) return;

		AddReward(.1f);
		if (dashing)
		{
			AddReward(.02f);
		}
		gameGroup.event_GoalScored.Invoke(teamID);

		gameGroup.DestroyGold(other.gameObject.transform);
	}
	private void OnCollisionEnter(Collision other)
	{
		//"GameAgent_0" or "GameAgent_1" means blue & red team
		if (!other.gameObject.tag.Contains("GameAgent_" + teamID_Opponent) && dashing)
		{
			AddReward(.02f);
		}
	}
}
