using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AddCoffeeMokaController : MonoBehaviour
{

    public GameObject coffee;
    public GameObject funnel;
    public GameObject funnelOrderLabel;

    int itemsPlaced = 0;
    private bool correctOrder = false;

    public void ObjectPlaced()
    {
        itemsPlaced += 1;

        if(itemsPlaced == 1)
        {
            if(coffee.activeSelf == true)
            {
                //funnel placed first
                correctOrder = true;
            }
        }

        if (coffee.activeSelf || itemsPlaced == 2)
        {
            funnelOrderLabel.SetActive(false);
        }

    }

    public void LevelFinished()
    {
        List<string> comments = new List<string>();
        int curScoreTotal = 10;
        int curScore = curScoreTotal;

        if(itemsPlaced == 0)
        {
            curScore = 0;
            comments.Add("No funnel or coffee.");
        }
        else if(itemsPlaced == 1)
        {
            if (!coffee.activeSelf)
            {
                //no funnel placed
                comments.Add("No funnel.");
                curScore = curScoreTotal / 3;
            }
            else
            {
                //no funnel placed
                comments.Add("No coffee.");
                curScore = curScoreTotal / 3;
            }
        }
        else
        {
            if (!correctOrder)
            {
                comments.Add("Wrong order. Funnel first.");
                curScore = curScoreTotal / 2;
            }
        }

        SingleScore myScore = new SingleScore(curScore, curScoreTotal, comments);

        this.SendMessageUpwards("StopLevel", myScore);
    }
}
