using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //other.TryGetComponent(out Agent agent);
        other.TryGetComponent(out AgentsMonoBehavior agentMono);

        //change mesh in scriptable object
        agentMono.agent.changToCube();

        //update mesh, coliider, and color in monobehavior
        agentMono.updateInfo();
    }
}
