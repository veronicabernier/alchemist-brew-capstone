using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScaleController : MonoBehaviour
{
    public TextMeshProUGUI weightDisplay;
    double curWeightDisplayed = 0.040;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Tare()
    {
        curWeightDisplayed = 0;
        weightDisplay.text = curWeightDisplayed.ToString("F3") + " g";
    }

    public void AddWeight(float weightAdded)
    {
        curWeightDisplayed += weightAdded;
        weightDisplay.text = curWeightDisplayed.ToString("F3") + " g";
    }

    public void RemoveWeight(float weightRemoved)
    {
        curWeightDisplayed -= weightRemoved;
        weightDisplay.text = curWeightDisplayed.ToString("F3") + " g";
    }
}
