using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputManager : MonoBehaviour
{
    public Vector2 inputVector {  get; private set; }
    public Vector3 mousePosition { get; private set; }
    // Update is called once per frame

    void start()
    {
        Service.inputManager = this;
    }
    public void UpdateManual()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        inputVector = new Vector2(h, v);//get the game input every frame
        mousePosition = Input.mousePosition;
    }
}
