using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class AgentsMonoBehavior : MonoBehaviour
{
    public Agent agent;
    public int teamID;
    public Mesh initialMesh;
    public Color initialColor;
    //public float speed;
    private bool isCD = false;
    private Material mat;
    private BehaviorTree.Tree<AgentsMonoBehavior> _tree;
    private UIController ui;


    public void coolDown() 
    {
        isCD = true;
    }
    void Start()
    {
        ui = FindObjectOfType<UIController>();
        mat = GetComponent<Renderer>().material;
        teamID = agent.teamID;
        //reset position based on scriptable object
        this.transform.position = new Vector3(agent.initial_X, 1f, 0f);
        Initialize();
        initializeTree();
        InvokeRepeating("coolDown", 1f, UnityEngine.Random.Range(1, 3));
    }

    public void updateInfo()
    {
        //change mesh
        GetComponent<MeshFilter>().mesh = agent.Mesh;
        //change collider
        GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;
        //change color
        mat.SetColor("_Color", agent.Color);

    }
    public void Initialize()
    {
        
        GetComponent<MeshFilter>().mesh = initialMesh;
        
        GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;
        
        mat.SetColor("_Color",initialColor );
    }

    public void initializeTree()
    {
        _tree = new Tree<AgentsMonoBehavior>
            (
                new Selector<AgentsMonoBehavior>
                (
                    new Sequence<AgentsMonoBehavior>
                    (
                        new isTeamZero(),
                        new moveingUp()
                     ),
                    new movingDown()
                 )
            );
    }

    public class isTeamZero : BehaviorTree.Node<AgentsMonoBehavior>
    {
        public override bool Update(AgentsMonoBehavior context)
        {
            if (context.teamID == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public void moveUp()
    {
        if (isCD == true) 
        {
            this.transform.Translate(new Vector3(0, 0, 0.5f), Space.World);
            isCD = false;
        }
    }

    

    public void moveDown() 
    {
        if (isCD == true) 
        {
            this.transform.Translate(new Vector3(0, 0, -0.5f), Space.World);
            isCD = false;

        }
    }

  

    public class moveingUp : BehaviorTree.Node<AgentsMonoBehavior> 
    {
        public override bool Update(AgentsMonoBehavior context)
        {
            context.moveUp();
            return true;
        }
    }

    public class movingDown : BehaviorTree.Node<AgentsMonoBehavior> 
    {
        
        public override bool Update(AgentsMonoBehavior context)
        {
            context.moveDown();
            return true;
        }
    }

    private void Update()
    {
        _tree.Update(this);
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall")) 
        {
            if (teamID == 0)
            {
                ui.team1Account += 1;
                teamID = 1;
            }
            else 
            {
                ui.team2Account += 1;
                teamID = 0;
            }
        }
        ui.updateUI();
    }
}
