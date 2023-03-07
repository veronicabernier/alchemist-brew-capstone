using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLiquid : MonoBehaviour
{
    public GameObject spriteMask;
    public GameObject lineGuide;

    //public float yStart;
    public float yEnd;
    [Tooltip("Per Second")]
    public float ySpeed = 0.001f;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            FillLiquid();
        }
    }

    private void FillLiquid()
    {
        float curY = spriteMask.transform.localPosition.y;
        if(curY < yEnd)
        {
            float newY = Mathf.Min(curY + ySpeed, yEnd);
            spriteMask.transform.localPosition = new Vector3(spriteMask.transform.localPosition.x, newY);
        }
    }

    private void OnMouseDown()
    {

    }

}
