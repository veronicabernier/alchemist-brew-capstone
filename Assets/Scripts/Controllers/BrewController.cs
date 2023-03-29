using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BrewController : MonoBehaviour
{

    public GameObject progressBar;
    public SpriteMask waterMask;

    private bool filling = false;
    private ProgressBar pb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (filling)
        {
            waterMask.transform.localPosition = new Vector3(waterMask.transform.localPosition.x, 1 - pb.progressAmount.fillAmount, waterMask.transform.localPosition.z);
        }
    }

    public void ObjectPlaced()
    {
        progressBar.SetActive(true);
        pb = progressBar.GetComponent<ProgressBar>();
        filling = true;
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
