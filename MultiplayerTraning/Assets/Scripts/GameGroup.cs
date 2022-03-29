using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Policies;
using System.Linq;
using TMPro;

public class GameGroup : MonoBehaviour
{
    //define the max step in the virtual environment
    [Tooltip("Max Environment Steps")] 
    public int MaxEnvironmentSteps = 1500;
    //current environment in this episode
    private int currentEnvironmentStep;

    [SerializeField]
    Transform prefab_Gold;

    [HideInInspector]
    public List<Transform> currentGolds = new List<Transform>();//to store all golds in the sccene

    [SerializeField]
    List<Transform> spawnerList_Gold = new List<Transform>();//to store all gold spawner positions

    //time to control gold shower
    float goldTimer = 0;
    float goldTimerTotal = 3;

    //declare ai teams
    private SimpleMultiAgentGroup m_BlueAgentGroup;
    private SimpleMultiAgentGroup m_RedAgentGroup;

    [SerializeField]
    List<MinerAgent> gameAgents;
    List<MinerAgent> gameAgents_TeamBlue = new List<MinerAgent>();
    List<MinerAgent> gameAgents_TeamRed = new List<MinerAgent>();


    //current scores
    int redScore = 0;
    int blueScore = 0;

    [SerializeField]
    TextMeshPro tX_RedScore;
    [SerializeField]
    TextMeshPro tX_BlueScore;


    //features and use of delegate funjctions
    public delegate void Event_GoalScored(int teamIDScored);
    public Event_GoalScored event_GoalScored;

    // Start is called before the first frame update
    void Start()
    {
        //set up the AgentGroup
        m_BlueAgentGroup = new SimpleMultiAgentGroup();
        m_RedAgentGroup = new SimpleMultiAgentGroup();


        //add listener to GoalScored event
        event_GoalScored += OnGoalScored;

        foreach (MinerAgent agent in gameAgents)
        {
            if (agent.GetComponent<BehaviorParameters>().TeamId == 0)//which is blue team
            {
                m_BlueAgentGroup.RegisterAgent(agent);//add this agent into this AgentGroup
                gameAgents_TeamBlue.Add(agent);
            }
            else
            {
                m_RedAgentGroup.RegisterAgent(agent);
                gameAgents_TeamRed.Add(agent);
            }
        }

        ResetScene();
    }

    private void OnDestroy()
    {
        event_GoalScored -= OnGoalScored;//remove the listener
    }

    void FixedUpdate()
    {
        currentEnvironmentStep += 1;
        if (currentEnvironmentStep >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
           
        {
            //game is over, calculat the result
            if (blueScore > redScore)//blue wins
            {
                m_BlueAgentGroup.AddGroupReward(1f);
                m_RedAgentGroup.SetGroupReward(-1f);
                //use SetGroupReward rather than AddGroupReward
                //use final reward to replace the reward gained by agents
                m_BlueAgentGroup.EndGroupEpisode();
                m_RedAgentGroup.EndGroupEpisode();
            }
            else if (blueScore < redScore)
            {
                //red team win 
                m_RedAgentGroup.AddGroupReward(1f);
                m_BlueAgentGroup.SetGroupReward(-1f);
                m_BlueAgentGroup.EndGroupEpisode();
                m_RedAgentGroup.EndGroupEpisode();
            }
            else
            {
                //draw game
                //set rewards to both team to be 0
                m_RedAgentGroup.SetGroupReward(0);
                m_BlueAgentGroup.SetGroupReward(0);
                m_BlueAgentGroup.EndGroupEpisode();
                m_RedAgentGroup.EndGroupEpisode();
            }

            //after result calculation, reset scene
            ResetScene();
        }
		else
        //when the game is still runing 
        {
            m_BlueAgentGroup.AddGroupReward(-0.0002f / (blueScore + 1));
            m_RedAgentGroup.AddGroupReward(-0.0002f / (redScore + 1));
        }
    }


	void ResetScene()
    {
        //reset environment
        currentEnvironmentStep = 0;
        
        //reset score
        blueScore = 0;
        redScore = 0;

        //text ui reset
        RefreshText();

        //Destroy all golds
        CleanupAllGold();
    }


    //triggered by MinerAgent when an agent grabed a gold
    void OnGoalScored(int teamIDScored)
    {
        if (teamIDScored == 0)
        {
            blueScore++;
            m_BlueAgentGroup.AddGroupReward(0.1f);
        }
        else
        {
            redScore++;
            m_RedAgentGroup.AddGroupReward(0.1f);
        }
        RefreshText();
    }


    //whenever socre change, trigger this functio
    void RefreshText()
	{
        tX_BlueScore.text = "" + blueScore;
        tX_RedScore.text = "" + redScore;
    }

    //**Gold Spawning
    private void Update()
    {
        if (goldTimer > goldTimerTotal)
        {
            //spawn
            goldTimer = 0;
            GoldShower();

        }
        else
		{
            goldTimer += Time.deltaTime;

        }
    }

    void GoldShower()
    {
        for(int i = 0; i < 2; i++)
        {
            Transform spawner = spawnerList_Gold[Random.Range(0, spawnerList_Gold.Count)];
            Transform gold = Instantiate(prefab_Gold, transform);
            gold.transform.position = spawner.position;
            currentGolds.Add(gold);
        }
    }

    void CleanupAllGold()
	{
        foreach(Transform gold in currentGolds)
		{
            Destroy(gold.gameObject);
		}

        currentGolds.Clear();
    }

    //controlled by MinerAgent, remove specific gold when the agent grab a gold
    public void DestroyGold(Transform gold)
    {
        //remove this gold from the gold list
        currentGolds.Remove(gold);

        //destroy this gold
        Destroy(gold.gameObject);
    }
}
