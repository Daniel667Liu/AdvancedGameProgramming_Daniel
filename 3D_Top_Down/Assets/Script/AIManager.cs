using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager
{
  
    public List<AIController> AIs;
    // Start is called before the first frame update
    public void Initialize()
    {
        Service.aiManager = this;
        AIs = new List<AIController>();
        
    }
    
    public void targetUpdate() 
    {
        if (Service.collectableManager.itemsList.Count > 1)
        {
            for (int i = 0; i < AIs.Count; i++)
            {
                /* if (AIs[i].targetFinished==true)
                    {
                        AIs[i].targetChoose();
                        AIs[i].targetFinished = false;
                    }*/  //bug possible:2 ai choose 1 target, when cube was destroyed, the pointer of target in ai wiil be null!
                AIs[i].targetChoose();
            }
        }
        else
        {
            Service.gameManager.is_AI_Tracking = false;
        }
        
    }

    // Update is called once per frame
    public void UpdateManual()
    {
        for (int i = 0; i < AIs.Count; i++)
        {
            AIs[i].UpdateManual();
        }
    }

    public void Ondestroy() 
    {
        for (int i = 0; i < AIs.Count; i++) 
        {
            AIs[i].destroySelf();
            AIs.Clear();
        }
    }
}
    
