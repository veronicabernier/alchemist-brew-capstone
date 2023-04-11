using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementBar : MonoBehaviour
{
    //bar
    public GameObject bar;
    public GameObject safeZone;
    public GameObject position;
    //fill bar
    public Image fillProgress;
    public float fillSpeed = 0.01f;
    //rotation change speed (works with z axis to connect with bar right now)
    public float rotationZSpeed;
    public float rotationZMax;
    public GameObject objectToChange;

    public GameObject animatedImage;
    public Transform animatedImagePos;
    public GameObject coffeeFull;
    public bool interacting = false;

    public float y0Value = 0f;

    private float curSafeMinY;
    private float curSafeMaxY;

    private static Vector3 safeZoneTotalMin;
    private static Vector3 safeZoneTotalMax;

    private static float ogZRotation;

    private static Vector3 minPosition;
    private static Vector3 maxPosition;

    private static float ogCoffeeScaleY;
    private static float ogCoffeePosY;
    // Start is called before the first frame update
    void Start()
    {
        ogZRotation = objectToChange.transform.rotation.eulerAngles.z;
        ogCoffeeScaleY = coffeeFull.transform.localScale.y;
        ogCoffeePosY = coffeeFull.transform.localPosition.y;

        safeZoneTotalMax = new Vector3(safeZone.transform.localPosition.x, bar.GetComponent<RectTransform>().sizeDelta.y / 2 - safeZone.GetComponent<RectTransform>().sizeDelta.y / 2 - 0.05f, safeZone.transform.localPosition.z);
        safeZoneTotalMin = new Vector3(0, y0Value, 0) + new Vector3(safeZone.transform.localPosition.x, (-bar.GetComponent<RectTransform>().sizeDelta.y / 2) + safeZone.GetComponent<RectTransform>().sizeDelta.y / 2 + 0.05f, safeZone.transform.localPosition.z);
        safeZone.transform.localPosition = safeZoneTotalMin;

        curSafeMinY = safeZone.transform.localPosition.y - (safeZone.GetComponent<RectTransform>().sizeDelta.y/2);
        curSafeMaxY = safeZone.transform.localPosition.y + (safeZone.GetComponent<RectTransform>().sizeDelta.y / 2);

        maxPosition = new Vector3(position.transform.localPosition.x, bar.GetComponent<RectTransform>().sizeDelta.y / 2 - position.GetComponent<RectTransform>().sizeDelta.y / 2 - 0.05f, position.transform.localPosition.z);
        minPosition = new Vector3(position.transform.localPosition.x, (-bar.GetComponent<RectTransform>().sizeDelta.y / 2) + position.GetComponent<RectTransform>().sizeDelta.y / 2 + 0.05f, position.transform.localPosition.z);
        position.transform.localPosition = minPosition;

        animatedImage.GetComponent<Image>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!interacting)
        {
            BarUnactions();
        }
        if(position.transform.localPosition.y < curSafeMaxY && position.transform.localPosition.y > curSafeMinY)
        {
            Debug.Log("safe");
            fillProgress.fillAmount += fillSpeed;
            coffeeFull.transform.localScale = new Vector3(coffeeFull.transform.localScale.x, (1 - fillProgress.fillAmount) * ogCoffeeScaleY, coffeeFull.transform.localScale.z);
            coffeeFull.transform.localPosition = new Vector3(coffeeFull.transform.localPosition.x, (1 - fillProgress.fillAmount) * ogCoffeePosY, coffeeFull.transform.localPosition.z);
            animatedImage.GetComponent<Image>().enabled = (fillProgress.fillAmount < 1);
            animatedImage.transform.localPosition = animatedImagePos.position;

            //update safe zone
            safeZone.transform.localPosition = safeZoneTotalMin + (safeZoneTotalMax-safeZoneTotalMin)*fillProgress.fillAmount;

            curSafeMinY = safeZone.transform.localPosition.y - (safeZone.GetComponent<RectTransform>().sizeDelta.y / 2);
            curSafeMaxY = safeZone.transform.localPosition.y + (safeZone.GetComponent<RectTransform>().sizeDelta.y / 2);
        }
        else
        {
            Debug.Log("danger");
            animatedImage.GetComponent<Image>().enabled = false;
        }

    }

    public void BarActions()
    {
        Vector3 curAngles = objectToChange.transform.rotation.eulerAngles;
        Vector3 newAngles = new Vector3(curAngles.x, curAngles.y, Mathf.Min(curAngles.z + rotationZSpeed, rotationZMax));
        objectToChange.transform.eulerAngles = newAngles;

        float changeRatio = Math.Abs(newAngles.z - ogZRotation) / Math.Abs(rotationZMax - ogZRotation);
        position.transform.localPosition = minPosition + new Vector3(0, changeRatio * Math.Abs(minPosition.y - maxPosition.y), 0);
    }

    public void BarUnactions()
    {
        Vector3 curAngles = objectToChange.transform.rotation.eulerAngles;
        Vector3 newAngles = new Vector3(curAngles.x, curAngles.y, Mathf.Max(curAngles.z - rotationZSpeed, ogZRotation));
        objectToChange.transform.eulerAngles = newAngles;

        float changeRatio = Math.Abs(newAngles.z - ogZRotation) / Math.Abs(rotationZMax - ogZRotation);
        position.transform.localPosition = minPosition + new Vector3(0, changeRatio * Math.Abs(minPosition.y - maxPosition.y), 0);
    }

    public void LevelFinished()
    {
        List<string> comments = new List<string>();

        int curScoreTotal = 10;
        int curScore = (int)(fillProgress.fillAmount * curScoreTotal);
        if(curScore < curScoreTotal)
        {
            comments.Add("Didn't finish serving !");
        }
        SingleScore myScore = new SingleScore(curScore, curScoreTotal, comments);

        this.SendMessageUpwards("StopLevel", myScore);
    }
}
