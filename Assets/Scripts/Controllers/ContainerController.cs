using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ContainerController : MonoBehaviour
{
    public ScaleController sc;
    public float beanSetWeight = 0.20f;
    public Transform plateBeanParent;

    [System.NonSerialized]
    public bool platePlaced = false;

    private int curBean = 0;

    private List<GameObject> beanSets = new List<GameObject>();
    private List<GameObject> beanSetsPlate = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        PopulateBeanLists();
    }

    public double GenerateWantedWeight()
    {
        int beanCount = Math.Min(transform.childCount, plateBeanParent.childCount);
        int numBeans = UnityEngine.Random.Range(3, beanCount);

        return numBeans * beanSetWeight;
    }

    private void PopulateBeanLists()
    {
        foreach (Transform ch in GetComponentsInChildren<Transform>().Skip(1))
        {
            beanSets.Add(ch.gameObject);
        }
        foreach (Transform ch in plateBeanParent.GetComponentsInChildren<Transform>(true).Skip(1))
        {
            beanSetsPlate.Add(ch.gameObject);
        }
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
        if (curBean < beanSets.Count && curBean < beanSetsPlate.Count)
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
