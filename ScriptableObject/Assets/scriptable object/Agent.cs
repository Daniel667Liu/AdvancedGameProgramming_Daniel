using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName ="agent")]
public class Agent : ScriptableObject
{
    public Color Color;
    public Color red;
    public Color blue;
    public Mesh Mesh;
    public Mesh Cube;
    public Mesh Sphere;
    public int teamID;
    public float initial_X;

    public void changToCube() 
    {
        Mesh = Cube;
        Color = blue;

    }

    public void changToSphere() 
    {
        Mesh = Sphere;
        Color = red;
    }
}
