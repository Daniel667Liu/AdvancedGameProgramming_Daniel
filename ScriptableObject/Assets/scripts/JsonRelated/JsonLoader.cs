using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;


public class JSONLoader : MonoBehaviour
{
    public string filePath = "fileName.json";
    public delegate void JSONRefreshed();
    public JSONRefreshed jsonRefreshed;
    public JSONNode currentJSON;
    // Start is called before the first frame update
    void Start()
    {
        startRefreshJSON();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startRefreshJSON() 
    {
        refreshJSON();
    }

    void refreshJSON() 
    {
        string currentReadingPath = filePath;

        //open a file in the giving path, read all text and convert thenm into string, then close file
        string jsonText = File.ReadAllText(Application.streamingAssetsPath + currentReadingPath);

        //to parse json file, to read info and create object
        currentJSON = JSON.Parse(jsonText);
        if (jsonRefreshed != null) 
        {
            jsonRefreshed.Invoke();
        }
    }
}
