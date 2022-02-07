using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public int blueScore;
    public int redScore;
    public gameManager game { get; private set; }
    public GameObject TitleScene;
    public GameObject EndScene;

    //get the reference of every game states, used for input info from the scene
    gameBaseState currentState;
    gameStartState gameStartState = new gameStartState();
    gameIngameState gameIngameState = new gameIngameState();
    gameEndState gameEndState = new gameEndState();
    void Start()
    {
        Service.ServiceInitialize();
        Service.ServiceStart();
        currentState = gameStartState;
        currentState.EnterState(this);
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        Service.ServiceUpdate();
        currentState.UpdateState(this);
    }

    public void transitState( gameBaseState next) //used for transit into next game state
    {
        currentState.ExitState(this);
        next.EnterState(this);
        currentState = next;
    }

    public void GameStart() 
    {
        
        transitState(gameIngameState);
    }

    public void GameFinish() 
    {
        transitState(gameEndState);
        
    }

    public void GameRestart() 
    {
        transitState(gameStartState);
    }
}
