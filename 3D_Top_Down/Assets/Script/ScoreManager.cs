using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager 
{
    public int redScore { get; private set; }
    public int blueScore { get; private set; }
    public void Initialize() 
    {
        Service.scoreManager = this;
        EventManager.RegisterListener("redScored", redScored);
        EventManager.RegisterListener("blueScored", blueScored);
        redScore = 0;
        blueScore = 0;
    }

    private void blueScored()
    {
        blueScore += 1;
    }

    private void redScored()
    {
        redScore += 1;
    }

    public void Ondestroy() 
    {
        EventManager.UnregisterListener("redScored", redScored);
        EventManager.UnregisterListener("blueScored", blueScored);
        redScore = 0;
        blueScore = 0;
    }
}
