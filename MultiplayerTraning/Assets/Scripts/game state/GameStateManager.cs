using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;
using TMPro;

public class GameStateManager : MonoBehaviour
{
    public int blueScore;
    public int redScore;
    //public gameManager game { get; private set; }
    public GameObject TitleScene;
    public GameObject EndScene;

    public GameGroup gameGroup;

    //get the reference of every game states, used for input info from the scene
    gameBaseState currentState;
    gameStartState gameStartState = new gameStartState();
    gameIngameState gameIngameState = new gameIngameState();
    public gameEndState gameEndState = new gameEndState();

    public List<Transform> RedAgentSpawnPos;
    public List<Transform> BlueAgentSpawnPos;

    public MinerAgent redMinerPrefab;
    public MinerAgent blueMinerPrefab;

    public TextMeshProUGUI finalText;
    

    //calls when exiting the game start state
   /* public void SpawnMiners() 
    {
        gameGroup.m_BlueAgentGroup = new SimpleMultiAgentGroup();
        gameGroup.m_RedAgentGroup = new SimpleMultiAgentGroup();
        foreach (Transform pos in RedAgentSpawnPos) 
        {
            Debug.Log("red spanwed");
            //instantiate miner
            MinerAgent redMiner = GameObject.Instantiate(redMinerPrefab, pos);
            //add miner into list
            gameGroup.gameAgents_TeamRed.Add(redMiner);
            //regster ai in ai groups
            gameGroup.m_RedAgentGroup.RegisterAgent(redMiner);
        }

        foreach (Transform pos in BlueAgentSpawnPos) 
        {
            MinerAgent blueMiner = GameObject.Instantiate(blueMinerPrefab, pos);
            gameGroup.gameAgents_TeamBlue.Add(blueMiner);
            gameGroup.m_BlueAgentGroup.RegisterAgent(blueMiner);
        }

        gameGroup.gameBeign = true;

    }*/

    //calls when exiting game end state
    /*
    public void killMiners() 
    {
        //unregister every agents
        foreach (MinerAgent redMiner in gameGroup.gameAgents_TeamRed) 
        {
            gameGroup.m_RedAgentGroup.UnregisterAgent(redMiner);
        }
        //clear the list
        gameGroup.gameAgents_TeamRed.Clear();

        foreach (MinerAgent blueMiner in gameGroup.gameAgents_TeamBlue) 
        {
            gameGroup.m_BlueAgentGroup.UnregisterAgent(blueMiner);
        }
        gameGroup.gameAgents_TeamBlue.Clear();
    }*/
    void Start()
    {
        Debug.Log("statemanager initialized");
        //Service.ServiceInitialize();
        //Service.ServiceStart();
        currentState = gameStartState;
        currentState.EnterState(this);
        //gameGroup = FindObjectOfType<GameGroup>();
       // EventManager.RegisterListener("gameBegin", GameStart);
        EventManager.RegisterListener("gameFinish", GameFinish);
       // EventManager.RegisterListener("gameRestart", GameRestart);

        
    }

    // Update is called once per frame
    void Update()
    {
        redScore = gameGroup.redScore;
        blueScore = gameGroup.blueScore;
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
        //Debug.Log("game begin");
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

    public void OnDestroy()
    {
       // EventManager.UnregisterListener("gameBegin", GameStart);
        EventManager.UnregisterListener("gameFinish", GameFinish);
       // EventManager.UnregisterListener("gameRestart", GameRestart);
    }
}
