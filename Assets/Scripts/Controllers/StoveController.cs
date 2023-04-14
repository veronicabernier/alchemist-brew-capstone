using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoveController : MonoBehaviour
{
    //1. place mokapot 
    //2. choose heat
    //3. cook

    [System.Serializable]
    public enum HeatSetting
    {
        Low,
        Medium,
        High
    }

    public TextMeshProUGUI prompt;
    public GameObject progressBar;
    public GameObject heatSettings;

    public GameObject timer;
    public HeatSetting correctHeat = HeatSetting.Medium;


    private bool choseCorrectly = false;
    private bool placedMokapot;


    public void SelectHeat(string heat)
    {
        choseCorrectly = heat == correctHeat.ToString();
        
        heatSettings.SetActive(false);
        prompt.text = "Wait...";
        timer.SetActive(false);
        progressBar.SetActive(true);
    }

    public void ObjectPlaced()
    {
        //mokapot placed on stove
        heatSettings.SetActive(true);
        prompt.text = "Choose Heat!";
        placedMokapot = true;
    }

    public void ProgressDone()
    {
        List<string> comments = new List<string>();
        int totalScore = 10;
        int curScore = totalScore;
        if (!choseCorrectly)
        {
            comments.Add("Wrong Heat");
            curScore = totalScore/2;
        }

        SingleScore myScore = new SingleScore(curScore, totalScore, comments);

        this.SendMessageUpwards("StopLevel", myScore);
    }

    public void LevelFinished()
    {
        List<string> comments = new List<string>();
        int totalScore = 10;
        int curScore = totalScore;
        if (!placedMokapot)
        {
            //didnt place reservoir
            comments.Add("No Mokapot placed");
            comments.Add("No heat selected.");
            comments.Add("Time's Up!");
            curScore = 0;
        }
        else 
        {
            if (!choseCorrectly)
            {
                comments.Add("No heat selected.");
                comments.Add("Time's Up!");
                curScore = totalScore/3;
            }
        }


        SingleScore myScore = new SingleScore(curScore, totalScore, comments);

        this.SendMessageUpwards("StopLevel", myScore);
    }
}
