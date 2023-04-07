using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DripBrewController : MonoBehaviour
{

    public GameObject progressBar;
    public GameObject timer;
    public SpriteMask coffeeMask;

    private ProgressBar pb;

    int itemsPlaced = 0;

    // Update is called once per frame
    void Update()
    {
        if (pb)
        {
            coffeeMask.transform.localPosition = new Vector3(coffeeMask.transform.localPosition.x, pb.progressAmount.fillAmount, coffeeMask.transform.localPosition.z);
        }
    }

    public void ObjectPlaced()
    {
        itemsPlaced += 1;

        if(itemsPlaced == 2)
        {
            timer.SetActive(false);
            progressBar.SetActive(true);
            pb = progressBar.GetComponent<ProgressBar>();
        }
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
