using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    // Start is called before the first frame update

    private inputManager inputs;

    [SerializeField]
    private float moveSpeed;
    private Vector3 lookAtVector;

    private void Awake()
    {
        inputs = GetComponent<inputManager>();//get the input manager on awake the component
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveVector = new Vector3(inputs.inputVector.x, 0f, inputs.inputVector.y);
        
        playerMove(moveVector);
        playerLookAt();
    }

    private void playerLookAt()
    {
        Ray ray = Camera.main.ScreenPointToRay(inputs.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 500f)) 
        {
            Vector3 hitPoint = hitInfo.point;
            hitPoint.y = 0f;
            transform.LookAt(hitPoint);
        }
      
    }

    private void playerMove(Vector3 moveVector) 
    {
        float speed = moveSpeed *Time.deltaTime;//distance moved in a scaled time
        transform.Translate(speed*moveVector);//**p1 translate move, p2 relative coordination**
    }
}
