using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Service 
{
    
    public static inputManager inputManager;
    
    public static gameManager gameManager;
    
    public static collectableManager collectableManager;
    public static AIManager aiManager;
    public static void ServiceInitialize()
    {
        
        aiManager = new AIManager();
        collectableManager = new collectableManager();
        //gameManager = new gameManager();
        inputManager = new inputManager();

    }

    public static void ServiceStart() 
    {
        aiManager.Initialize();
        collectableManager.Initialize();
        inputManager.Initialize();
    }
    
    
}
