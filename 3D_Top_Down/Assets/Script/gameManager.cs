using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager 
{

    [SerializeField]
    private AIController aiPrefab;
    [SerializeField]
    private collectableItems itemPrefab;
   
    [HideInInspector]
    public bool is_AI_Tracking = true;
    // Start is called before the first frame update
    public void Initialize()
    {
        
        Service.gameManager = this;
        Debug.Log("gamanager initialize");
        aiPrefab =  GameObject.FindObjectOfType<AIController>();
        itemPrefab = GameObject.FindObjectOfType<collectableItems>();
    }
    public void spawn()
    {
        Service.aiManager.spawnAI();
        Service.collectableManager.spawnItem();
        
    }
    
    void Start()
    {
        
       
    }

    // Update is called once per frame
    public void UpdateManual()
    {
       
        

        if (is_AI_Tracking)
        {

            Service.aiManager.UpdateManual();
        }

        }

    

    


    private void OnDestroy()//unregister all listeners from sub system when destroy
    {
        Service.aiManager.Ondestroy();
        Service.collectableManager.Ondestroy();
        Service.scoreManager.Ondestroy();
    }
}
