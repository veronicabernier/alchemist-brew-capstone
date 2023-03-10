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
        }

        int curScore;

        if (!lc.enabled)
        {
            curScore = (int)(0.5f + (settingTotal * 0.5f));
        }
        else
        {
            int totalBeans = lc.GetBeanCount();
            int beansGround = lc.GetBeansGrounded();

            float errorQuantity = ((Mathf.Abs(beansGround - totalBeans) / (float)totalBeans) * curScoreTotal);
            curScore = (int)((curScoreTotal - errorQuantity) * 0.5f + (settingTotal * 0.5f));
        }


        SingleScore myScore = new SingleScore(curScore, curScoreTotal);


        this.SendMessageUpwards("StopLevel", myScore);
    }
}
