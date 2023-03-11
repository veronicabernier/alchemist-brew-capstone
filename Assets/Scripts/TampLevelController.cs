using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TampLevelController : MonoBehaviour
{
    public GameObject tamper;
    public GameObject distributer;
    public GameObject portafilter;

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
            //tamper is done
            tamper.GetComponent<SnapIntoPlace>().RevertToOriginal();
            tamper.GetComponent<SpriteRenderer>().color = new Color(0.55f, 0.55f, 0.55f, 0.8f);
            tamper.GetComponent<Press>().pressable = false;
        }
        else
        {
            //portafilter tapping is done
            portafilter.GetComponent<Press>().pressable = false;
            //activate tamper
            tamper.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            tamper.GetComponent<Drag>().dragIsActive = true;
        }
    }

    public void FinishedDistribution()
    {
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
}
