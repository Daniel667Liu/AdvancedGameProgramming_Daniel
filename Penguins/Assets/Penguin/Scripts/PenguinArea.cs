using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.MLAgents;
using System;

public class PenguinArea : MonoBehaviour
{
    [Tooltip("The agent inside the area")]
    public PenguinAgent penguinAgent;

    [Tooltip("The baby penguin inside the area")]
    public GameObject penguinBaby;

    [Tooltip("The TextMeshPro text that shows the cumulative reward of the agent")]
    public TextMeshPro cumulativeRewardText;

    [Tooltip("Prefab of a live fish")]
    public Fish fishPrefab;

    private List<GameObject> fishList;


    private void Start()
    {
        ResetArea();
    }

    private void Update()
    {
        //read the penguiAgent MlAgent reward for text mesh pro UI
        cumulativeRewardText.text = penguinAgent.GetCumulativeReward().ToString("0.00");
    }

    public void ResetArea ()
    {
        RemoveAllFish();
        PlacePenguin();
        PlaceBaby();
        SpawnFish(4, .5f);
    }

    private void SpawnFish(int count , float fishSpeed)
    {
        for (int i = 0; i < count; i++)
        {
            // Spawn and place the fish
            GameObject fishObject = Instantiate<GameObject>(fishPrefab.gameObject);
            fishObject.transform.position = ChooseRandomPosition(transform.position, 100f, 260f, 2f, 13f) + Vector3.up * .5f;
            fishObject.transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);

            // Set the fish's parent to this area's transform
            fishObject.transform.SetParent(transform);

            // Keep track of the fish
            fishList.Add(fishObject);

            // Set the fish speed
            fishObject.GetComponent<Fish>().fishSpeed = fishSpeed;
        }
    }

    //place the baby penguin in the area, same as the PlacePenguin
    private void PlaceBaby()
    {
        Rigidbody rigidbody = penguinBaby.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        penguinBaby.transform.position = ChooseRandomPosition(transform.position, -45f, 45f, 4f, 9f) + Vector3.up * .5f;
        penguinBaby.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    //place the penguin in the area
    private void PlacePenguin()
    {
        //get the rigid body of penguin
        Rigidbody rigidbody = penguinAgent.GetComponent<Rigidbody>();

        //reset the velocity of peguin
        rigidbody.velocity = Vector3.zero;

        //reset the angularvelocity of penguin, or it will rotate itself each episode
        rigidbody.angularVelocity = Vector3.zero;

        //randomly set the position and rotation for the penguin
        penguinAgent.transform.position = ChooseRandomPosition(transform.position, 0f, 360f, 0f, 9f) + Vector3.up * .5f;
        penguinAgent.transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
    }

    private void RemoveAllFish()
    {
        if (fishList != null) 
        {
            for (int i = 0; i < fishList.Count; i++) 
            {
                if (fishList[i] != null) 
                {
                    Destroy(fishList[i]);//if this fish exsit , then destroy it
                }
            }
        }

        fishList = new List<GameObject>();// build a new list for fish list
    }

    //remove a fish, remove from the list then destroy the fish
    public void RemoveSpecificFish(GameObject fishObject)
    {
        fishList.Remove(fishObject);
        Destroy(fishObject);
    }

    //get the number of fish remaining through fish list
    public int FishRemaining
    {
        get { return fishList.Count; }
    }


    //choose a random position by setting distances and angles in a relative coordinate
    public static Vector3 ChooseRandomPosition(Vector3 center, float minAngle, float maxAngle, float minRadius, float maxRadius)
    {
        float radius = minRadius;
        float angle = minAngle;

        if (maxRadius > minRadius)
        {
            // Pick a random radius
            radius = UnityEngine.Random.Range(minRadius, maxRadius);
        }

        if (maxAngle > minAngle)
        {
            // Pick a random angle
            angle = UnityEngine.Random.Range(minAngle, maxAngle);
        }

        // Center position + forward vector rotated around the Y axis by "angle" degrees, multiplies by "radius"
        // converting a relative coordinate into a normal one and return vec3 position
        return center + Quaternion.Euler(0f, angle, 0f) * Vector3.forward * radius;
    }
}
