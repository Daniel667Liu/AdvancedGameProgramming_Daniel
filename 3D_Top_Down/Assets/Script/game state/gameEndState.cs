using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameEndState : gameBaseState
{
    public override void EnterState(GameStateManager stateManager)
    {//define the winner of the game
        if (stateManager.redScore > stateManager.blueScore)
        {
            Debug.Log("red wins!");
        }
        else if (stateManager.redScore > stateManager.blueScore)
        {
            Debug.Log("tie game!");
        }
        else 
        {
            Debug.Log("blue wins!");
        }
    }
    public override void UpdateState(GameStateManager stateManager)
    {
        
    }

    public override void ExitState(GameStateManager stateManager)
    {
        Service.aiManager.Ondestroy();
        Service.collectableManager.Ondestroy();//clear all items and ais
    }
}
