using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WetGroundsController : MonoBehaviour
{
    //1. add grounds
    //2. fill water
    //3. bloom

    public GameObject grounds;
    public GameObject groundsPlaced;
    public GameObject chemex;

    public GameObject waterDoneButton;

    public TextMeshProUGUI prompt;
    public GameObject progressBar;

    public GameObject timer;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void WaterAdded()
    {
        prompt.text = "Wait!";
        waterDoneButton.SetActive(false);
        chemex.GetComponent<AddLiquid>().enabled = false;
        progressBar.SetActive(true);

        timer.SetActive(false);
    }

    public void ObjectPlaced()
    {
        //grounds placed
        groundsPlaced.SetActive(true);
        chemex.GetComponent<AddLiquid>().enabled = true;
        waterDoneButton.SetActive(true);
        prompt.text = "Wet Grounds!";
    }

    public void ProgressDone()
    {
        List<string> comments = new List<string>();
        int totalScore = 10;
        int curScore = totalScore;

        if(groundsPlaced.activeSelf == false)
        {
            curScore = 0;
            comments.Add("Missing grounds");
            comments.Add("Missing water");
        }
        else
        {
            SingleScore alScore = chemex.GetComponent<AddLiquid>().GetCurrentScore();
            curScore = (int)Math.Min(totalScore * 0.5 + ((float)alScore.curScore /(float)alScore.curScoreTotal) * 0.5 *totalScore, totalScore);
            foreach(string c in alScore.comments)
            {
                comments.Add(c);
            }
        }
        SingleScore myScore = new SingleScore(curScore, totalScore, comments);

        this.SendMessageUpwards("StopLevel", myScore);
    }

    public void LevelFinished()
    {
        ProgressDone();
    }
}