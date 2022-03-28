using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System;

public class PenguinAgent : Agent
{
    //declare virables
    [Tooltip("How fast the agent moves forward")]
    public float moveSpeed = 5f;

    [Tooltip("How fast the agent turns")]
    public float turnSpeed = 180f;

    [Tooltip("Prefab of the heart that appears when the baby is fed")]
    public GameObject heartPrefab;

    [Tooltip("Prefab of the regurgitated fish that appears when the baby is fed")]
    public GameObject regurgitatedFishPrefab;

    private PenguinArea penguinArea;
    new private Rigidbody rigidbody;

    //define the baby penguin gameobject
    private GameObject baby;

    //define if the babypenguin is full
    private bool isFull;


    //initialize the agent
    public override void Initialize()
    {
        base.Initialize();
        penguinArea = GetComponentInParent<PenguinArea>();
        baby = penguinArea.penguinBaby;
        rigidbody = GetComponent<Rigidbody>();
    }

    //when agent take actions based on data in model or training
    public override void OnActionReceived(ActionBuffers actions)
    {
        //calculate the data and convert into movement values first

        // the first action in the buffer would be moving forward
        float forwardAmount = actions.DiscreteActions[0];

        float turnAmount = 0f;
        //transfer the second action in the buffer into turning action
        if (actions.DiscreteActions[1] == 1f)
        {
            turnAmount = -1f;
        }
        if (actions.DiscreteActions[1] == 2f)
        {
            turnAmount = 1f;
        }

        //apply to movement
        rigidbody.MovePosition(transform.position
            + transform.forward * forwardAmount * moveSpeed * Time.fixedDeltaTime);
        transform.Rotate(transform.up * turnAmount * turnSpeed * Time.fixedDeltaTime);
        //summary: original position + delta position


        //add negtive reaward at each end of step to encourage actions
        if (MaxStep > 0)
        {
            AddReward(-1f / MaxStep);
        }
    }

    //when player take the wheel
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int forwardAction = 0;
        int turnAction = 0;

        if (Input.GetKey(KeyCode.W)) 
        {
            forwardAction = 1;
        }

        //trun left
        if (Input.GetKey(KeyCode.A)) 
        {
            turnAction = 1;
        }

        //turn right
        if (Input.GetKey(KeyCode.D)) 
        {
            turnAction = 2;
        }

        //put actions into the action buffer array
        actionsOut.DiscreteActions.Array[0] = forwardAction;
        actionsOut.DiscreteActions.Array[1] = turnAction;
    }

    //act when each training episode begin
    public override void OnEpisodeBegin()
    {
        //the baby isnt full, reset the area
        isFull = false;
        penguinArea.ResetArea();
    }

    //collect data from the scene
    public override void CollectObservations(VectorSensor sensor)
    {
        //observe if the baby is full, 1 space for bool
        sensor.AddObservation(isFull);

        //observe the distance from the baby penguin, 1 space for float
        sensor.AddObservation(Vector3.Distance(baby.transform.position, transform.position));

        //observe the direction of the baby. 3 spaces for vector3
        sensor.AddObservation((baby.transform.position - transform.position).normalized);

        //observe the direction that itself is facing, 3 spaces for vector3
        sensor.AddObservation(transform.forward);

        //total spaces needed is 1+1+3+3=8.

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("fish"))
        {
            // Try to eat the fish
            EatFish(collision.gameObject);
        }
        else if (collision.transform.CompareTag("baby"))
        {
            // Try to feed the baby
            RegurgitateFish();
        }
    }

    private void RegurgitateFish()
    {
        if (!isFull) return; // Nothing to regurgitate
        isFull = false;

        // Spawn regurgitated fish
        GameObject regurgitatedFish = Instantiate<GameObject>(regurgitatedFishPrefab);
        regurgitatedFish.transform.parent = transform.parent;
        regurgitatedFish.transform.position = baby.transform.position;
        Destroy(regurgitatedFish, 4f);

        // Spawn heart
        GameObject heart = Instantiate<GameObject>(heartPrefab);
        heart.transform.parent = transform.parent;
        heart.transform.position = baby.transform.position + Vector3.up;
        Destroy(heart, 4f);

        AddReward(1f);

        if (penguinArea.FishRemaining <= 0)
        {
            EndEpisode();
        }
    }

    private void EatFish(GameObject gameObject)
    {
        if (isFull) return; // Can't eat another fish while full
        isFull = true;

        penguinArea.RemoveSpecificFish(gameObject);

        AddReward(1f);
    }
}
