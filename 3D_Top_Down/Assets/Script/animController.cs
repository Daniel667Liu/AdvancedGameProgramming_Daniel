using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    private Rigidbody rigidBody;

    private void Start()
    {
        rigidBody = GetComponentInChildren<Rigidbody>();//make sure there is only 1 rigidbody
    }

    // Update is called once per frame
    void Update()
    {
        if (!(rigidBody.velocity.magnitude == 0))
        {
            animator.SetBool("isRUnning", true);
        }
        else 
        {
            animator.SetBool("isRunning", false);
        }
    }
}
