using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeighingController : MonoBehaviour
{
    public ContainerController cc;
    public ScaleController sc;
    public TextMeshProUGUI wantedDisplay;

    private double wantedWeight;
    // Start is called before the first frame update
    void Start()
    {
        wantedWeight = cc.GenerateWantedWeight();
        wantedDisplay.text = "Weigh " + wantedWeight.ToString("F3") + " g" + " of beans!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LevelFinished()
    {
        double actualWeight = cc.GetActualBeanWeight();

        //determine score
        int curScoreTotal = 10;
        int errorQuantity = (int) ((System.Math.Abs(actualWeight - wantedWeight) / wantedWeight)*curScoreTotal);
        int curScore = Math.Max(curScoreTotal - errorQuantity, 0);
        List<string> comments = new List<string>();
        if(curScore != curScoreTotal)
        {
            if(actualWeight < wantedWeight)
            {
                comments.Add("Missing beans :(");
            }
            else
            {
                comments.Add("To many beans!");
            }
           
        }
        
        SingleScore myScore = new SingleScore(curScore, curScoreTotal, comments);


        this.SendMessageUpwards("StopLevel", myScore);
    }
}
