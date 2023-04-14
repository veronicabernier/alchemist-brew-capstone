using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChooseFilterController : MonoBehaviour
{
    //1. add reservoir
    //2. choose filter
    //3. blind cycle

    public GameObject filters;
    public GameObject paperFilter;
    public GameObject permanentFilter;

    public TextMeshProUGUI filterPrompt;
    public GameObject progressBar;
    public SpriteMask waterMask;

    public GameObject timer;

    private string correctFilter = "";
    private string prompt = "";
    private bool choseCorrectly = false;

    private ProgressBar pb;
    private bool running = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void GenerateCorrectFilter()
    {
        int i = UnityEngine.Random.Range(0, 2);
        if(i == 0)
        {
            correctFilter = "paper";
            prompt = "Clear coffee";
        }
        else
        {
            correctFilter = "permanent";
            prompt = "Strong coffee";
        }
        filterPrompt.text = prompt;
    }

    // Update is called once per frame
    void Update()
    {
        if (running)
        {
            waterMask.transform.localPosition = new Vector3(waterMask.transform.localPosition.x, pb.progressAmount.fillAmount, waterMask.transform.localPosition.z);
        }
    }

    public void ObjectPlaced()
    {
        if(filters.activeSelf == false)
        {
            //reservoir placed
            filters.SetActive(true);
            GenerateCorrectFilter();
        }
        else
        {
            //filter chosen
            if (paperFilter.activeInHierarchy == false)
            {
                //paper chosen
                choseCorrectly = (correctFilter == "paper");
                permanentFilter.SetActive(false);
            }
            else
            {
                //permanent chosen
                choseCorrectly = (correctFilter == "permanent");
                paperFilter.SetActive(false);
            }
            progressBar.SetActive(true);
            pb = progressBar.GetComponent<ProgressBar>();
            running = true;
            timer.SetActive(false);
        }

    }

    public void ProgressDone()
    {
        List<string> comments = new List<string>();
        int totalScore = 10;
        int curScore = totalScore;
        if (!choseCorrectly)
        {
            comments.Add("Wrong Filter");
            curScore -= (int)((1.0f / 3.0f) * totalScore);
        }

        SingleScore myScore = new SingleScore(curScore, totalScore, comments);

        this.SendMessageUpwards("StopLevel", myScore);
    }

    public void LevelFinished()
    {
        List<string> comments = new List<string>();
        int totalScore = 10;
        int curScore = totalScore;
        if (!filters.activeSelf)
        {
            //didnt place reservoir
            comments.Add("Missing Reservoir");
            comments.Add("No Filter");
            comments.Add("Time's Up!");
            curScore = 0;
        }
        else 
        {
            if (!running)
            {
                comments.Add("No filter");
                comments.Add("Time's Up!");
                curScore -= (int)((2.0f / 3.0f) * totalScore);
            }
            else
            {
                if (!choseCorrectly)
                {
                    comments.Add("Wrong Filter");
                    curScore -= (int)((1.0f / 3.0f) * totalScore);
                }
            }
        }


        SingleScore myScore = new SingleScore(curScore, totalScore, comments);

        this.SendMessageUpwards("StopLevel", myScore);
    }
}
