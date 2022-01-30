using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectableItems : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (!(other.GetComponent<AIController>() == null)) 
        {
            Service.collectableManager.UpdateItemList(this);//remove collected item in the list
            //other.GetComponent<AIController>().targetFinished = true;
            Service.aiManager.targetUpdate();//update target for all ai
            Destroy(this.gameObject);
        }

        if (!(other.GetComponent<playerController>() == null))
        {
            Service.collectableManager.UpdateItemList(this);
            Service.aiManager.targetUpdate();
            Destroy(this.gameObject);
        }
    }

   
}
