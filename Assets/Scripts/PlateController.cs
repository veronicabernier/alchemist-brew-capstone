using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateController : MonoBehaviour
{
    public ScaleController sc;
    public ContainerController cc;

    public float minWeight = 0.10f;
    public float maxWeight = 0.40f;

    bool placed = false;

    public void ObjectPlaced()
    {
        sc.AddWeight(Random.Range(minWeight, maxWeight));
        placed = true;
        cc.platePlaced = true;
    }

    public void OnMouseUp()
    {
        if(placed)
        {
            cc.MoveToContainer();
        }
    }
}
