using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private collectableItems target;
    [SerializeField]
    private float speed;
    //public bool targetFinished;
    public void targetChoose() //choose one of the items positions, randomly
    {
        List<collectableItems> Targets = Service.collectableManager.itemsList;
        int i = Random.Range(0, Targets.Count);
        target = Targets[i];
        Debug.Log(target.gameObject.transform.position);
    }

    public void trackTarget(collectableItems target) //translate ai to the target position
    {
        
            Vector3 dir = (target.gameObject.transform.position - transform.position).normalized;
            transform.position += dir * speed * Time.deltaTime;
        
        
    
        
    }


 
    void Start()
    {
        targetChoose();
    }

    // Update is called once per frame
    public void UpdateManual()
    {
        trackTarget(target);
    }
}
