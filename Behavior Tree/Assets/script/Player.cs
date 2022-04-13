using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int rank { get; private set; }

    [SerializeField]
    private float speed = 1f;
   
    // Start is called before the first frame update
    void Start()
    {
        rank = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W)) 
        {
            
            this.transform.Translate(Vector3.forward * speed * Time.deltaTime,Space.World);
        }

        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(Vector3.left * speed * Time.deltaTime,Space.World);
        }

        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(Vector3.back * speed * Time.deltaTime,Space.World);
        }

        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(Vector3.right * speed * Time.deltaTime,Space.World);
        }

        if (Input.GetKeyDown(KeyCode.J)) 
        {
            rank = 0;
        }

        if (Input.GetKeyDown(KeyCode.K)) 
        {
            rank = 2;
        }
    }
}
