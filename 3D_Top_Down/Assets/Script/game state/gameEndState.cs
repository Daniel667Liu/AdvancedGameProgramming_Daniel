using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameEndState : gameBaseState
{
    public override void EnterState(GameStateManager stateManager)
    {//define the winner of the game
        EventManager.UnregisterListener("gameFinished", stateManager.GameFinish);
        stateManager.EndScene.SetActive(true);
        //Text text = stateManager.EndScene.GetComponent<Text>();
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
        
        stateManager.EndScene.SetActive(false);//clear all items and ais
    }
}
