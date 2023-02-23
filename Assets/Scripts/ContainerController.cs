using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerController : MonoBehaviour
{
    public ScaleController sc;
    public GameObject[] beanSets;
    public GameObject[] beanSetsPlate;
    public float beanSetWeight = 0.20f;

    public bool platePlaced = false;

    int curBean = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseUp()
    {
        if(platePlaced)
        {
            MoveToPlate();
        }
    }

    void MoveToPlate()
    {
        if (curBean < beanSets.Length && curBean < beanSetsPlate.Length)
        {
            beanSets[curBean].SetActive(false);
            beanSetsPlate[curBean].SetActive(true);
            sc.AddWeight(beanSetWeight);
            curBean += 1;
        }
    }

    public void MoveToContainer()
    {
        if(curBean > 0)
        {
            curBean -= 1;
            beanSetsPlate[curBean].SetActive(false);
            beanSets[curBean].SetActive(true);
            sc.RemoveWeight(beanSetWeight);
        }
    }
}
