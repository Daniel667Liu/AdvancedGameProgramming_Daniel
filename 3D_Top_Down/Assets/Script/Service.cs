using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Service 
{
    
    public static inputManager inputManager;
    
    public static gameManager gameManager;
    
    public static collectableManager collectableManager;
    public static AIManager aiManager;
    public static ScoreManager scoreManager;
   // public static EventManager eventManager;
    public static void ServiceInitialize()
    {
        scoreManager = new ScoreManager();
        collectableManager = new collectableManager();

        aiManager = new AIManager();
        //gameManager = new gameManager();
        inputManager = new inputManager();

        //eventManager = new EventManager();

    }

    public static void ServiceStart() 
    {
        aiManager.Initialize();
        collectableManager.Initialize();
        inputManager.Initialize();
        scoreManager.Initialize();
       
    }
    
    
}
