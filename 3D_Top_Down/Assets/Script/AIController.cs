using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private Vector3 targetPosition;
    public void targetChoose() //choose one of the items positions, randomly
    {
        List<Vector3> Targets = Service.collectableManager.PositionCal();
        int i = Random.Range(0, Targets.Count);
        targetPosition = Targets[i];
    }

    public void trackTarget(Vector3 target) //translate ai to the target position
    {
        this.gameObject.transform.Translate(target);
    }


 
    void Start()
    {
        targetChoose();
    }

    // Update is called once per frame
    public void UpdateManual()
    {
        trackTarget(targetPosition);
    }
}
