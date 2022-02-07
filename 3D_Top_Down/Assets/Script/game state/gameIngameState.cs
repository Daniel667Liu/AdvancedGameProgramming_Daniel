using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameIngameState : gameBaseState
{
    public override void EnterState(GameStateManager stateManager)
    {
        Service.gameManager.spawn();
        //stateManager.game.gameObject.SetActive(true);//activate game manager, spawn ais and items
    }
    public override void UpdateState(GameStateManager stateManager)
    {
        stateManager.redScore = Service.scoreManager.redScore;//update the scores every frame
        stateManager.blueScore = Service.scoreManager.blueScore;

    }

    public override void ExitState(GameStateManager stateManager)
    {
        Service.aiManager.Ondestroy();
        Service.collectableManager.Ondestroy();
        //stateManager.game.gameObject.SetActive(false);//deactivate the gama manager, pause game
    }
}
