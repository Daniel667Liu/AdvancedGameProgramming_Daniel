using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager 
{
    public List<AIController> AIs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    

    // Update is called once per frame
    public void UpdateManual()
    {
        for (int i = 0; i < AIs.Count; i++)
        {
            AIs[i].UpdateManual();
        }
    }
    public void targetUpdate() //update the tracking positions for all ais
    {
        for (int i = 0; i < AIs.Count; i++) 
        {
            AIs[i].targetChoose();
        }
    }
}
