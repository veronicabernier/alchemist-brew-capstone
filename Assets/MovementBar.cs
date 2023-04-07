using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementBar : MonoBehaviour
{
    //bar
    public GameObject bar;
    public GameObject saveZone;
    public GameObject position;
    //rotation change speed (works with z axis to connect with bar right now)
    public float rotationZSpeed;
    public float rotationZMax;
    public GameObject objectToChange;
    //position -> not necessary for now
    //acceleration
    //range of error
    public float saveZoneAmount = 0.4f;
    //speed of change
    //range of change

    public bool interacting = false;

    private float ogZRotation;
    private Vector3 minPosition;
    private Vector3 maxPosition;

    // Start is called before the first frame update
    void Start()
    {
        ogZRotation = objectToChange.transform.rotation.eulerAngles.z;
        saveZone.GetComponent<Image>().fillAmount = saveZoneAmount;
        maxPosition = new Vector3(position.transform.localPosition.x, saveZone.GetComponent<RectTransform>().sizeDelta.y / 2 - position.GetComponent<RectTransform>().sizeDelta.y / 2, position.transform.localPosition.z);
        minPosition = new Vector3(position.transform.localPosition.x, (-saveZone.GetComponent<RectTransform>().sizeDelta.y / 2) + position.GetComponent<RectTransform>().sizeDelta.y / 2, position.transform.localPosition.z);
        position.transform.localPosition = minPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!interacting)
        {
            BarUnactions();
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
}
