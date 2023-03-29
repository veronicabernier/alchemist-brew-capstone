using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TampLevelController : MonoBehaviour
{
    public GameObject tamper;
    public GameObject distributer;
    public GameObject portafilter;

    //distribute, tap, tamp
    private int totalRequiredParts = 3;
    private int curCompletedParts = 0;

    private bool distributerDone = false;
    private bool tapsDone = false;
    private bool tampingDone = false;

    // Start is called before the first frame update
    void Start()
    {
        tamper.GetComponent<Drag>().dragIsActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PressesDone()
    {
        if(tamper.GetComponent<Press>().pressable == true)
        {
            curCompletedParts = 3;
            //tamper is done
            tampingDone = true;
            tamper.GetComponent<SnapIntoPlace>().RevertToOriginal();
            tamper.GetComponent<SpriteRenderer>().color = new Color(0.55f, 0.55f, 0.55f, 0.8f);
            tamper.GetComponent<Press>().pressable = false;
        }
        else
        {
            curCompletedParts = 2;
            //portafilter tapping is done
            tapsDone = true;
            portafilter.GetComponent<Press>().pressable = false;
            //activate tamper
            tamper.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            tamper.GetComponent<Drag>().dragIsActive = true;
        }
    }

    public void FinishedDistribution()
    {
        distributerDone = true; 

        curCompletedParts = 1;
        distributer.GetComponent<SpriteRenderer>().color = new Color(0.55f, 0.55f, 0.55f, 0.8f);
        distributer.GetComponent<Drag>().dragIsActive = false;
        distributer.GetComponent<SnapIntoPlace>().RevertToOriginal();

        tamper.GetComponent<SpriteRenderer>().sortingOrder = distributer.GetComponent<SpriteRenderer>().sortingOrder + 1;

        portafilter.GetComponent<Press>().pressable = true;
    }

    public void ObjectPlaced()
    {
        tamper.GetComponent<Press>().pressable = true;
        tamper.GetComponent<Drag>().dragIsActive = false;
    }

    public void LevelFinished()
    {
        //determine score
        int curScoreTotal = 10;

        int curScore = (int) (((float) curCompletedParts/ (float) totalRequiredParts) * curScoreTotal);
        List<string> comments = new List<string>();
        if (!distributerDone)
        {
            comments.Add("Unfinished distribution");
        }
        if (!tapsDone)
        {
            comments.Add("Unfinished portafilter tapping");
        }
        if (!tampingDone)
        {
            comments.Add("Unfinished tamping");
        }

        SingleScore myScore = new SingleScore(curScore, curScoreTotal, comments);


        this.SendMessageUpwards("StopLevel", myScore);
    }
}
