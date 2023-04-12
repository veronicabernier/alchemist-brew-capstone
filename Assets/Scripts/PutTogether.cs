using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PutTogether : MonoBehaviour
{

    public GameObject[] parts;

    int partsPlaced = 0;
    private bool correctOrder = true;

    public void ObjectPlaced()
    {
        partsPlaced += 1;

        if(correctOrder)
        {
            //each time an object is placed is an opportunity for fail
            //this will always check when the current number of item is the one placed
            correctOrder = !parts[partsPlaced - 1].GetComponent<Drag>().dragIsActive;
        }

        foreach(GameObject p in parts)
        {
            if (!p.GetComponent<Drag>().dragIsActive)
            {
                p.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }

    public void LevelFinished()
    {
        List<string> comments = new List<string>();
        int curScoreTotal = 10;
        int curScore = curScoreTotal;

        if(partsPlaced == 0)
        {
            curScore = 0;
            comments.Add("No parts put together.");
        }
        else if(partsPlaced < parts.Length)
        {
            comments.Add("Missing parts.");
            curScore = (int)(((float)partsPlaced / (float)parts.Length)*curScoreTotal*0.5f);
        }
        else
        {
            if (!correctOrder)
            {
                comments.Add("Wrong order.");
                curScore = curScoreTotal / 2;
            }
        }

        SingleScore myScore = new SingleScore(curScore, curScoreTotal, comments);

        this.SendMessageUpwards("StopLevel", myScore);
    }
}
