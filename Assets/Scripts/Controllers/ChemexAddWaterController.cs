using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChemexAddWaterController : MonoBehaviour
{
    //1. generate line pos y
    //2. fill water
    //3. filter

    public GameObject chemex;

    public GameObject waterDoneButton;

    public TextMeshProUGUI prompt;
    public GameObject progressBar;

    public GameObject timer;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void WaterAdded()
    {
        prompt.text = "Wait";
        waterDoneButton.SetActive(false);
        chemex.GetComponent<AddLiquid>().enabled = false;
        progressBar.SetActive(true);

        timer.SetActive(false);
    }


    public void ProgressDone()
    {
        this.SendMessageUpwards("StopLevel", chemex.GetComponent<AddLiquid>().GetCurrentScore());
    }

    public void LevelFinished()
    {
        ProgressDone();
    }
}
