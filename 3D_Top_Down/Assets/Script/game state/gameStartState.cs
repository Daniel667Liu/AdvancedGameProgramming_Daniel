using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameStartState :gameBaseState
{
    public override void EnterState(GameStateManager stateManager)
    {
        EventManager.RegisterListener("gameFinished", stateManager.GameFinish);
        //show the title scene
        stateManager.TitleScene.SetActive(true);

        //Service.gameManager.Initialize();
        //Service.gameManager.spawn();
    }
    public override void UpdateState(GameStateManager stateManager)
    {
        
    }

    public override void ExitState(GameStateManager stateManager)
    {
        //close the titlen scene
        stateManager.TitleScene.SetActive(false);
    }
}
