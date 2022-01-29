using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public Vector3 targetChoose() 
    {
        List<Vector3> Targets = Service.collectableManager.PositionCal();
        int i = Random.Range(0, Targets.Count);
        return Targets[i];
    }

    public void trackTarget(Vector3 target) 
    {
        this.gameObject.transform.Translate(target);
    }


 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
