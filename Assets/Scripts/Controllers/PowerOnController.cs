using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerOnController : MonoBehaviour
{

    public GameObject progressBar;
    public GameObject timer;


    public void ObjectPlaced()
    {
        timer.SetActive(false);
        progressBar.SetActive(true);
    }

    public void ProgressDone()
    {
        SingleScore myScore = new SingleScore(10, 10, new List<string>());

        this.SendMessageUpwards("StopLevel", myScore);
    }

    public void LevelFinished()
    {
        List<string> comments = new List<string>();
        comments.Add("Time's Up!");

        SingleScore myScore = new SingleScore(0, 10, comments);

        this.SendMessageUpwards("StopLevel", myScore);
    }
}
