using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectableManager 
{
    // Start is called before the first frame update
    public List<collectableItems> itemsList;
    
    public void Initialize ()
    {
        Service.collectableManager = this;
        itemsList = new List<collectableItems>();
    }

    
    

    public void UpdateItemList(collectableItems collectedItem) //remove the collected items
    {
        if (itemsList.Count > 1)
        {
            itemsList.Remove(collectedItem);
        }
        else 
        {
            itemsList.Clear();
        }
    }

    
    // Update is called once per frame
    public void UpdateManual()
    {
        if (itemsList.Count < 1) 
        {
            Service.gameManager.is_AI_Tracking = false;
        }
    }
}
