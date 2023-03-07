using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLiquid : MonoBehaviour
{
    public GameObject spriteMask;
    public GameObject lineGuide;
    public float lineYMin;
    public float lineYMax;

    private float yStart;
    public float yEnd;
    [Tooltip("Per Second")]
    public float ySpeed = 0.001f;


    private float correctYMask;
    

    // Start is called before the first frame update
    void Start()
    {
        yStart = spriteMask.transform.localPosition.y;
        PositionLine();
    }

    private void PositionLine()
    {
        float positionPercentage = UnityEngine.Random.Range(0.00f, 1.00f);
        float lineYPos = lineYMin + (Mathf.Abs(lineYMax - lineYMin) * positionPercentage);

        lineGuide.transform.localPosition = new Vector2(lineGuide.transform.localPosition.x, lineYPos);

        correctYMask = yStart + (Mathf.Abs(yEnd - yStart) * positionPercentage);
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
            Debug.Log("curY: " + newY.ToString());
            Debug.Log("correctY: " + correctYMask);
        }
    }

    private void OnMouseDown()
    {

    }

}
