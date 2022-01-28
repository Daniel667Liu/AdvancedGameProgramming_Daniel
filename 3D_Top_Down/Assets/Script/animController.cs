using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animController : MonoBehaviour
{
    [SerializeField]
    private Animator robotAnimator;
    private Rigidbody rigidBody;
    
    

    private void Start()
    {
        rigidBody = GetComponentInChildren<Rigidbody>();//make sure there is only 1 rigidbody
        Service.animController = this;
    }

    // Update is called once per frame
    public void UpdateManual()
    {
        if (Service.inputManager.inputVector.magnitude > 0)
        {
            robotAnimator.SetBool("isRunning", true);
        }
        else 
        {
            robotAnimator.SetBool("isRunning", false);
        }
        
    }
}
