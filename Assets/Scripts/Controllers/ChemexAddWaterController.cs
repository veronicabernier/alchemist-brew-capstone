using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChemexAddWaterController : MonoBehaviour
{

    public MovementBar mb;

    public GameObject waterDoneButton;

    public TextMeshProUGUI prompt;
    public GameObject progressBar;

    public SpriteMask waterMask;
    public float waterMaskMin;
    public float waterMaskMax;
    public SpriteRenderer placedGrounds;

    public SpriteMask coffeeMask;
    public float coffeeMaskMin;
    public float coffeeMaskMax;

    public GameObject timer;

    private ProgressBar pb;
    private float waterMaskReached;
    private float coffeeMaskReached;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (progressBar.activeSelf)
        {
            //filtering
            float newYPos = waterMaskMin + (1 - pb.progressAmount.fillAmount) * Math.Abs(waterMaskReached - waterMaskMin);
            waterMask.transform.localPosition = new Vector3(waterMask.transform.localPosition.x, newYPos, waterMask.transform.localPosition.z);

            newYPos = coffeeMaskMin + pb.progressAmount.fillAmount * Math.Abs(coffeeMaskReached - coffeeMaskMin);
            coffeeMask.transform.localPosition = new Vector3(coffeeMask.transform.localPosition.x, newYPos, coffeeMask.transform.localPosition.z);

        }
        else
        {
            //adding water
            float newYPos = waterMaskMin + mb.fillProgress.fillAmount * Math.Abs(waterMaskMax - waterMaskMin);
            waterMask.transform.localPosition = new Vector3(waterMask.transform.localPosition.x, newYPos, waterMask.transform.localPosition.z);
        }

    }

    public void WaterAdded()
    {
        prompt.text = "Wait!";
        waterDoneButton.SetActive(false);
        progressBar.SetActive(true);
        pb = progressBar.GetComponent<ProgressBar>();
        placedGrounds.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        waterMaskReached = waterMask.transform.localPosition.y;
        coffeeMaskReached = coffeeMaskMin + Math.Abs(coffeeMaskMax-coffeeMaskMin) * (1-(Math.Abs(waterMaskReached - waterMaskMax) / Math.Abs(waterMaskMax - waterMaskMin)));

        timer.SetActive(false);
    }

    public void ProgressDone()
    {
        mb.LevelFinished();
    }

}
