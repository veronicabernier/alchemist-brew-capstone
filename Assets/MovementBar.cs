using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementBar : MonoBehaviour
{
    //bar
    public GameObject saveZone;
    public GameObject position;
    //rotation change speed
    public Vector3 rotationSpeed;
    public Vector3 rotationMax;
    public GameObject objectToChange;
    //position -> not necessary for now
    //acceleration
    //range of error
    public float saveZoneHeight = 0.4f;
    //speed of change
    //range of change

    public bool interacting = false;

    private Vector3 ogRotation;

    // Start is called before the first frame update
    void Start()
    {
        ogRotation = objectToChange.transform.rotation.eulerAngles;
        saveZone.GetComponent<Image>().fillAmount = saveZoneHeight;
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
        Vector3 newAngles = new Vector3(Mathf.Min(curAngles.x + rotationSpeed.x, rotationMax.x), Mathf.Min(curAngles.y + rotationSpeed.y, rotationMax.y), Mathf.Min(curAngles.z + rotationSpeed.z, rotationMax.z));
        objectToChange.transform.eulerAngles = newAngles;
    }

    public void BarUnactions()
    {
        Vector3 curAngles = objectToChange.transform.rotation.eulerAngles;
        Vector3 newAngles = new Vector3(Mathf.Max(curAngles.x - rotationSpeed.x, ogRotation.x), Mathf.Max(curAngles.y - rotationSpeed.y, ogRotation.y), Mathf.Max(curAngles.z - rotationSpeed.z, ogRotation.z));
        objectToChange.transform.eulerAngles = newAngles;
    }
}
