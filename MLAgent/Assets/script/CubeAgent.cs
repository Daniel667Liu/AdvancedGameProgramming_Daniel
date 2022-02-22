using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;//fro observing
using Unity.MLAgents.Actuators;//for action buffers
using System;

public class CubeAgent : Agent
{
    //ball pn the cube's head
    public GameObject ball;
    Rigidbody m_BallRb;


    //only excute once when the training starts
    public override void Initialize()
    {
        m_BallRb = ball.GetComponent<Rigidbody>();
        SetBall();
    }



    private void SetBall()//
    {
        var scale = 1;
        m_BallRb.mass = scale;
        ball.transform.localScale = new Vector3(scale, scale, scale);
    }

    private void SetCube() //reset cube rotation
    {
        gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        gameObject.transform.Rotate(new Vector3(1, 0, 0), UnityEngine.Random.Range(-10f, 10f));
        gameObject.transform.Rotate(new Vector3(1, 0, 1), UnityEngine.Random.Range(-10f, 10f));
        m_BallRb.velocity = new Vector3(0, 0, 0);

        ball.transform.position = new Vector3(UnityEngine.Random.Range(-1.2f, 1.2f), 4f, UnityEngine.Random.Range(-1.2f, 1.2f)) + gameObject.transform.position;
    }

    public override void OnEpisodeBegin()//excute and the begining of a cycle
    {
        SetCube();
        SetBall();

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(m_BallRb.velocity);//speed and  direction of the ball, need a 3-size space
        sensor.AddObservation(gameObject.transform.rotation.z);//rotation X and Z of the cube. need a  2-size space
        sensor.AddObservation(gameObject.transform.rotation.x);
        sensor.AddObservation(ball.transform.position - gameObject.transform.position);
        //realative position between cube and ball, need a 3-size space
    }

    public override void OnActionReceived(ActionBuffers actions)//apply the actions from neural network
    {
        var actionZ = 2f * Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        var actionX = 2f * Mathf.Clamp(actions.ContinuousActions[1], -1f, 1f);

        gameObject.transform.Rotate(new Vector3(0, 0, 1), actionZ);//rotate around z
        gameObject.transform.Rotate(new Vector3(1, 0, 0), actionX);//rotate around x

        if (ball.transform.position.y - gameObject.transform.position.y < -3f//the ball fall down
             || Mathf.Abs(ball.transform.position.x - gameObject.transform.position.x) > 5f//too far in x
            || Mathf.Abs(ball.transform.position.z - gameObject.transform.position.z) > 5f)// too far in z
        {
            SetReward(-1f);
            EndEpisode();
        }
        else
        {
            SetReward(0.1f);
            //EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)//switch to human control
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }
}
