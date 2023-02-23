using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScaleController : MonoBehaviour
{
    public TextMeshProUGUI weightDisplay;
    double curWeight = 0.040;

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
        curWeight = 0;
        weightDisplay.text = curWeight.ToString("F3") + " g";
    }
}
