using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChooseWaterController : MonoBehaviour
{
    //1. choose water
    //2. fill water
    //3. blind cycle

    public GameObject chamber;

    public GameObject hotButton;
    public GameObject coldButton;
    public TextMeshProUGUI prompt;

    private string correctWater = "";
    private string waterSelected = "";

    // Start is called before the first frame update
    void Start()
    {
        GenerateCorrectWater();
    }

    private void GenerateCorrectWater()
    {
        int i = UnityEngine.Random.Range(0, 2);
        if (i == 0)
        {
            correctWater = "cold";
            prompt.text = "Add water for a slower process!";
        }
        else
        {
            correctWater = "hot";
            prompt.text = "Add water for a faster process!";
        }
    }

    public void Selected(string selection)
    {
        waterSelected = selection;

        hotButton.SetActive(false);
        coldButton.SetActive(false);

        Color oldColor = chamber.GetComponent<SpriteRenderer>().color;
        chamber.GetComponent<SpriteRenderer>().color = new Color(oldColor.r, oldColor.g, oldColor.b, 0.5f);
        chamber.GetComponent<AddLiquid>().enabled = true;
    }



    public void LevelFinished()
    {
        List<string> comments = new List<string>();
        int totalScore = 10;
        int curScore = totalScore;

        if (waterSelected == "")
        {
            curScore = 0;
            comments.Add("No water choice.");
            comments.Add("Missing water :{");
        }
        else
        {
            if(waterSelected != correctWater)
            {
                curScore = 0;
                comments.Add("Wrong water choice :{");
            }

            SingleScore alScore = chamber.GetComponent<AddLiquid>().GetCurrentScore();
            curScore = (int)Math.Min(curScore * 0.5 + ((float)alScore.curScore / (float)alScore.curScoreTotal) * 0.5 * totalScore, totalScore);
            foreach (string c in alScore.comments)
            {
                comments.Add(c);
            }
        }
        SingleScore myScore = new SingleScore(curScore, totalScore, comments);

        this.SendMessageUpwards("StopLevel", myScore);
    }
}

