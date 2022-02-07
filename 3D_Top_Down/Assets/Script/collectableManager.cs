using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectableManager 
{
    // Start is called before the first frame update
    public List<collectableItems> itemsList;
    private collectableItems itemPrefab;
    
    public void Initialize ()
    {
        Service.collectableManager = this;
        itemsList = new List<collectableItems>();
        EventManager.RegisterListener("Scored",Scored);
        itemPrefab = GameObject.FindObjectOfType<collectableItems>();
    }

    public void spawnItem()
    {
        //Debug.Log("spawn item");
        for (int i = 0; i < 15; i++)
        {

            Vector3 ItemPosition = new Vector3(Random.Range(-23, 23), 1, Random.Range(-23, 23));
            Quaternion rotation = new Quaternion(0, 0, 0, 0);
            collectableItems spawnedItem;
            spawnedItem = GameObject.Instantiate(itemPrefab, ItemPosition, rotation) as collectableItems;//spawn the item
            Service.collectableManager.itemsList.Add(spawnedItem);//add spawned itmes into list in collectableManager
        }
    }
    void Scored() 
    {
        Debug.Log("scored form item");
       
    }
    

    public void UpdateItemList(collectableItems collectedItem) //remove the collected items
    {
        if (itemsList.Count > 2)
        {
            itemsList.Remove(collectedItem);
        }
        else 
        {
            //Debug.Log("ssssssssssssss");
            //itemsList.Clear();
            EventManager.TiggerEvent("gameFinished");
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

    public void Ondestroy() 
    {
        EventManager.UnregisterListener("Scored", Scored);
        for (int i = 0; i < itemsList.Count; i++) 
        {
            itemsList[i].destroySelf();
        }
        itemsList.Clear();
    }
}
