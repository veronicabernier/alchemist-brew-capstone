using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ServeController : MonoBehaviour
{

    public GameObject progressBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ObjectPlaced()
    {
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
