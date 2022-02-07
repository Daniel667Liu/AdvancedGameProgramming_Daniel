using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectableItems : MonoBehaviour
{
    public void destroySelf() 
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!(other.GetComponent<AIController>() == null)) 
        {
            if (other.gameObject.GetComponent<AIController>().teamNumber == 0)
            {
                EventManager.TiggerEvent("redScored");
            }
            else
            {
                EventManager.TiggerEvent("blueScored");
            }//send the massage to the event manager to inform score manager increase scores
            Service.collectableManager.UpdateItemList(this);//remove collected item in the list
            //other.GetComponent<AIController>().targetFinished = true;
            Service.aiManager.targetUpdate();//update target for all ai
            Destroy(this.gameObject);
            
        }

        if (!(other.GetComponent<playerController>() == null))
        {

            EventManager.TiggerEvent("blueScored");//see player as the blue team
            Service.collectableManager.UpdateItemList(this);
            Service.aiManager.targetUpdate();
            Destroy(this.gameObject);
            
        }

       // Debug.Log("red: " + Service.scoreManager.redScore + " blue: " + Service.scoreManager.blueScore);
    }

   
}
