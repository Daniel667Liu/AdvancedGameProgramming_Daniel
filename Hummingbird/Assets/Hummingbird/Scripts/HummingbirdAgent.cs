using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;

/// <summary>
/// A hummingbird Machine Learning Agent
/// </summary>
public class HummingbirdAgent : Agent 
{

    //force apply to the bird
    float moveForce = 2f;
    //for rotattion, pitch direction
    float pitchSpeed = 100f;
    //for rotation, yaw direction
    float yawSpeed = 100f;
    public Transform beakTip;//bird's mouth tip
    public Camera agentCamera;//camera of the agent
    public bool trainingMode;//for gameMode trainingMode check
    FlowerManager flowerManager;//get access to the flower manager
    Flower nearestFlower;//store the nearest flower
    new Rigidbody rigidbody;//bird's rigidbody
    public float NectarObtained { get; private set; }
    private float smoothPitchChange;// for smooth the movement of rotation
    private float smoothYawChange;
    private const float MaxPitchAngle = 80f;
    private const float MaxYawAngle = 80f;
    private const float BeakTipRadius = 0.008f;//define the radius of the beaktip

    //whether the ai is frozen
    private bool frozen = false;

    public override void Initialize()
    {
        rigidbody = GetComponent<Rigidbody>();
        flowerManager = GetComponentInParent<FlowerManager>();

        if (!trainingMode) MaxStep = 0;
    }

    public override void OnEpisodeBegin()
    {
        if (trainingMode)
        {
            flowerManager.ResetFlowers();
        }
        NectarObtained = 0;

        //zero out veloity so that movement stops before a new episode begins
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        bool infrontOfFlower = true;
        if (infrontOfFlower) { 
            infrontOfFlower = UnityEngine.Random.value > .5f; 
        }

        //relocate the agent to a new safe position
        MoveToSafeRandomPosition(infrontOfFlower);

        //update the info of nearest flower
        UpdateNearestFlower();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (nearestFlower = null) 
        {
            sensor.AddObservation(new float[10]);
            return;
        }

        sensor.AddObservation(transform.localRotation.normalized);


        //direction to the flower
        Vector3 toFlower = nearestFlower.FlowerCenterPosition - beakTip.position;
        sensor.AddObservation(toFlower.normalized);

        //bird face to the flower or not
        sensor.AddObservation(Vector3.Dot(toFlower.normalized, -nearestFlower.FlowerUpVector.normalized));

        //beap face to the flower or not
        sensor.AddObservation(Vector3.Dot(beakTip.forward.normalized, -nearestFlower.FlowerUpVector.normalized));

        //realative distance between from beap to flower
        sensor.AddObservation(toFlower.magnitude / FlowerManager.AreaDiameter);//areaDiameter is size of the whole island
    }

    // ***unfinished***
    public override void OnActionReceived(ActionBuffers actions)
    {
        if (frozen) return;

        Vector3 move = new Vector3(actions.ContinuousActions[0], actions.ContinuousActions[1], actions.ContinuousActions[2]);
        rigidbody.AddForce(move * moveForce);

        Vector3 rotationVector = transform.rotation.eulerAngles;
        float pitchChange = actions.ContinuousActions[3];
        float yawChange = actions.ContinuousActions[4];

        smoothPitchChange = Mathf.MoveTowards(smoothPitchChange, pitchChange, 2f * Time.deltaTime);

        float pitch = rotationVector.x + smoothPitchChange * Time.fixedDeltaTime * pitchSpeed;
        if (pitch > 180f) pitch -= 360f;
        pitch = Mathf.Clamp(pitch, -MaxPitchAngle, MaxPitchAngle);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        
    }

    //move the agent to a safe position, inside the boundary wihtut touching rocks or bushes
    private void MoveToSafeRandomPosition(bool inFrontfFlower) 
    {
        bool safePositionFound = false;
        int attemptRemaining = 100;//to prevent the infinite loop
        Vector3 potentialPosition = Vector3.zero;
        Quaternion potentialRotation = new Quaternion();

        while (!safePositionFound && attemptRemaining > 0) 
        {
            attemptRemaining--;
            if (inFrontfFlower)
            {
                //pick a random flower
                Flower randomFlower = flowerManager.Flowers[UnityEngine.Random.Range(0, flowerManager.Flowers.Count)];

                //lovate at 10cm infront the flower
                float distanceFromFlower = UnityEngine.Random.Range(.1f, .2f);
                potentialPosition = randomFlower.transform.position + randomFlower.FlowerUpVector * distanceFromFlower;

                Vector3 toFlower = randomFlower.FlowerCenterPosition - potentialPosition;
                potentialRotation = Quaternion.LookRotation(toFlower, Vector3.up);

            }
            else 
            {
                float height = UnityEngine.Random.Range(1.2f, 2f);
                float radius = UnityEngine.Random.Range(2f, 7f);

                Quaternion direction = Quaternion.Euler(0f, UnityEngine.Random.Range(-180f, 180f), 0f);

                potentialPosition = flowerManager.transform.position + Vector3.up * height + direction * Vector3.forward * radius;
                float pitch = UnityEngine.Random.Range(-60f, 60f);
                float yaw = UnityEngine.Random.Range(-80f, 80f);

                potentialRotation = Quaternion.Euler (pitch, yaw, 0f);

            }

            Collider[] colliders = Physics.OverlapSphere(potentialPosition, 0.05f);

            safePositionFound = colliders.Length == 0;
        }

        transform.position = potentialPosition;
        transform.rotation = potentialRotation;
    }
    
    
    //update the info of nearest flower for the bird
    private void UpdateNearestFlower()
    {
        foreach (Flower flower in flowerManager.Flowers) 
        {

            if (nearestFlower == null && flower.HasNectar)
            {
                nearestFlower = flower;
            }

            else if (flower.HasNectar) 
            {
                float distanceToFlower = Vector3.Distance(flower.transform.position, beakTip.transform.position);
                float distanceToCurrentNearestFlower = Vector3.Distance(nearestFlower.transform.position, beakTip.transform.position);

                if (!nearestFlower.HasNectar || distanceToFlower < distanceToCurrentNearestFlower) 
                {
                    nearestFlower = flower;
                }
            }
        }
    }

    public void FreezeAgent() 
    {

    }

    public void UnfreezeAgent() 
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        TriggerEnterOrStay(other);
    }

    private void OnTriggerStay(Collider other)
    {
        TriggerEnterOrStay(other);
    }

    void TriggerEnterOrStay(Collider collider) 
    {
        if (collider.CompareTag("nectar")) 
        {
            // get the closest point to the beak on the flower
            Vector3 closestPointToBeakTip = collider.ClosestPoint(beakTip.position);

            //if the closest point is in the radius
            if (Vector3.Distance(beakTip.position, closestPointToBeakTip) < BeakTipRadius) 
            {
                Flower flower = flowerManager.GetFlowerFromNectar(collider);
                float nectarRecieved = flower.Feed(.1f);

                if (trainingMode) 
                {

                    //define the bonus by how close the angle between beak and the flower is
                    float bonus = 0.2f * Mathf.Clamp01(Vector3.Dot(transform.forward.normalized, -nearestFlower.FlowerUpVector.normalized));
                    AddReward(.1f + bonus);
                }

                if (!flower.HasNectar) 
                {
                    UpdateNearestFlower();
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (trainingMode && collision.collider.CompareTag("boundary")) 
        {
            //if touch the boundary
            AddReward(-0.5f);
        }
    }

    private void Update()
    {
      
    }

}
