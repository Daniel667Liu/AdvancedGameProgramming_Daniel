using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public int blueScore;
    public int redScore;
    public gameManager game { get; private set; }

    //get the reference of every game states, used for input info from the scene
    gameBaseState currentState;
    gameStartState gameStartState = new gameStartState();
    gameIngameState gameIngameState = new gameIngameState();
    gameEndState gameEndState = new gameEndState();
    void Start()
    {
        game = FindObjectOfType<gameManager>();
        currentState = gameStartState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void transitState( gameBaseState next) //used for transit into next game state
    {
        currentState.ExitState(this);
        next.EnterState(this);
        currentState = next;
    }
}
