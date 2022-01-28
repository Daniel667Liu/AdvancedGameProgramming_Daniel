using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Service 
{
    
    public static inputManager inputManager;
    public static playerController playerController;
    public static gameManager gameManager;
    public static animController animController;
    public static void ServiceInitialize ()
    {
        animController = new animController();
        gameManager = new gameManager();
        inputManager = new inputManager();
        playerController = new playerController();
    }

    
    
}
