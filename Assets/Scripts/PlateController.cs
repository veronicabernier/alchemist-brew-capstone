using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateController : MonoBehaviour
{
    public ScaleController sc;
    public float minWeight = 0.10f;
    public float maxWeight = 0.40f;

    public void ObjectPlaced()
    {
        sc.AddWeight(Random.Range(minWeight, maxWeight));
    }
}
