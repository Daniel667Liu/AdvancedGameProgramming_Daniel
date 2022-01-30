using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectableManager 
{
    // Start is called before the first frame update
    public List<collectableItems> itemsList;
    
    void Start ()
    {
        
    }

    
    

    public void UpdateItemList(collectableItems collectedItem) //remove the collected items
    {
        itemsList.Remove(collectedItem);
    }

    public List<Vector3> PositionCal() //return positions of items
    {
         List<Vector3> Positions = new List<Vector3>();
        for (int i = 0; i < itemsList.Count; i++) 
        {
            Positions.Add(itemsList[i].GetComponent<Transform>().position);
        }
        return Positions;
    }
    // Update is called once per frame
    void UpdateManual()
    {
        
    }
}
