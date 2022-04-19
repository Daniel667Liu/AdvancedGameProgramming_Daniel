using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

[System.Serializable]
public class teamInfo
{
    public string team1Name;
    public string team2Name;
    public int team1Account;
    public int team2Account;

    //read info from json file
    public static teamInfo ReadFromJSON(string path)
    {
        string jsonString = File.ReadAllText(Application.streamingAssetsPath + path);
        return JsonUtility.FromJson<teamInfo>(jsonString);
    }

    //write info into json file
    public static void saveToJSON(string path, teamInfo teamInfo)
    {
        string jsonString = JsonUtility.ToJson(teamInfo);
        File.WriteAllText(Application.streamingAssetsPath + path, jsonString);
    }
}
public class UIController : MonoBehaviour
{
    public string team1Name;
    public string team2Name;
    public int team1Account;
    public int team2Account;
    [SerializeField]
    private Text teamName1;
    [SerializeField]
    private Text teamName2;
    [SerializeField]
    private Text teamAccount1;
    [SerializeField]
    private Text teamAccount2;


    public void updateUI ()
    {
        teamName1.text = team1Name;
        teamName2.text = team2Name;
        teamAccount1.text = team1Account.ToString();
        teamAccount2.text = team2Account.ToString();
    }

    //save ui info into json file
    public void saveUI()
    {
        teamInfo info = new teamInfo();
        info.team1Name = this.team1Name;
        info.team2Name = this.team2Name;
        info.team1Account = this.team1Account;
        info.team2Account = this.team2Account;

        teamInfo.saveToJSON("/JSON/teamInfo.json", info);
    }

    //load return teaminfo from selected json file
    public teamInfo loadUI() 
    {
        return teamInfo.ReadFromJSON("/JSON/teamInfo.json");
    }

    public void loadButton() 
    {
        teamInfo loadedInfo = new teamInfo();
        loadedInfo = loadUI();
        this.team1Name = loadedInfo.team1Name;
        this.team1Account = loadedInfo.team1Account;
        this.team2Name = loadedInfo.team2Name;
        this.team2Account = loadedInfo.team2Account;
        this.updateUI();
    }

    [SerializeField]
    private Button loadButtonComponent;
    public void saveButton() 
    {
        saveUI();
        loadButtonComponent.interactable = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        updateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
