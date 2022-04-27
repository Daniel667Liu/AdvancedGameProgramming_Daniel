using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageRotate : MonoBehaviour
{
    Transform transform;
    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)) 
        {
            Debug.Log("sss");
            transform.Rotate(0f, Input.GetAxis("Mouse X"), 0f)
                ;
            transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), 0, 0),Space.World);
        }
    }

    public void CameraReset() 
    {
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }
}
