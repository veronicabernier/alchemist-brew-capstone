using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrindController : MonoBehaviour
{
    public GameObject container;
    public GameObject beansToGrindParent;
    public LeverRotate lc;
    public SnapToCloserX stcx;

    public SnapToCloserX.SettingType wantedGrindSetting;

    bool stcxDeactivated = false;

    // Start is called before the first frame update
    void Start()
    {
        lc.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!stcxDeactivated && lc.isDirty)
        {
            stcx.DeactivateSnap();
            stcxDeactivated = true;
        }
    }

    public void ObjectPlaced()
    {
        container.SetActive(false);
        beansToGrindParent.SetActive(true);
        lc.enabled = true;
    }

    public void LevelFinished()
    {
        SnapToCloserX.SettingType grindSettingUsed = stcx.GetSettingUsed();
        List<string> comments = new List<string>();

        //determine score
        int curScoreTotal = 10;

        int settingTotal;
        if(grindSettingUsed == wantedGrindSetting)
        {
            settingTotal = curScoreTotal;
        }
        else
        {
            settingTotal = 0;
            comments.Add("Wrong grind setting >:(");
        }

        int curScore;

        if (!lc.enabled)
        {
            curScore = (int)(0.5f + (settingTotal * 0.5f));
            comments.Add("No grinding done >:(");
        }
        else
        {
            int totalBeans = lc.GetBeanCount();
            int beansGround = lc.GetBeansGrounded();

            float errorQuantity = ((Mathf.Abs(beansGround - totalBeans) / (float)totalBeans) * curScoreTotal);
            curScore = (int)(Math.Max((curScoreTotal - errorQuantity), 0) * 0.5f + (settingTotal * 0.5f));

            if(beansGround < totalBeans)
            {
                comments.Add("You didn't grind all :(");
            }
        }


        SingleScore myScore = new SingleScore(curScore, curScoreTotal, comments);


        this.SendMessageUpwards("StopLevel", myScore);
    }
}
