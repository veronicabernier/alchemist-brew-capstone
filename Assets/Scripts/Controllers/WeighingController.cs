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
}
