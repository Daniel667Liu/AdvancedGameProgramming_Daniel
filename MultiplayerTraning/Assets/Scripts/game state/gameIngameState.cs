using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class gameIngameState : gameBaseState
{
    
    private int rs = 0;//red score
    private int bs = 0;//blue score
    public override void EnterState(GameStateManager stateManager)
    {
        
    }
    public override void UpdateState(GameStateManager stateManager)
    {

        rs = stateManager.redScore;
        bs = stateManager.blueScore;
        if (rs >= 7 || bs >= 7) 
        {
            EventManager.TiggerEvent("gameFinish");
        }
    }

    public override void ExitState(GameStateManager stateManager)
    {
        stateManager.EndScene.gameObject.SetActive(true);
        if (rs > bs)
        {
            stateManager.finalText.text= "RED WIN!";
        }
        else 
        {
            stateManager.finalText.text = "BLUE WIN!";
        }
       
    }
}
