using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Service.gameManager = this;
        Service.ServiceInitialize();
    }

    // Update is called once per frame
    void Update()
    {
        
        Service.inputManager.UpdateManual();
        Service.playerController.UpdateManual();
        Service.animController.UpdateManual();
        
    }
}
