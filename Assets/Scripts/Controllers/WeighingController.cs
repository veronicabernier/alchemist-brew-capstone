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

    public void LevelFinished()
    {
        double actualWeight = cc.GetActualBeanWeight();

        //determine score
        int curScoreTotal = 10;
        int curScore =  (int)(curScoreTotal - System.Math.Abs(actualWeight - wantedWeight)*2);

        SingleScore myScore = new SingleScore(curScore, curScoreTotal);
        //Debug.Log(curScore);

        this.SendMessageUpwards("StopLevel", myScore);
    }
}
