using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentsMonoBehavior : MonoBehaviour
{
    public Agent agent;
    public int teamID;
    public Mesh initialMesh;
    public Color initialColor;
    private Material mat;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        teamID = agent.teamID;
        //reset position based on scriptable object
        this.transform.position = new Vector3(agent.initial_X, 2f, 0f);
        Initialize();
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



}
