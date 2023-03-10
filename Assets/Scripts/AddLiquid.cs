using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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


    public float correctYMask;
    

    // Start is called before the first frame update
    void Start()
    {
        yStart = spriteMask.transform.localPosition.y;
        PositionLine();
    }

    private void PositionLine()
    {
        float positionPercentage = UnityEngine.Random.Range(0.10f, 0.90f);
        float lineYPos = lineYMin + (Mathf.Abs(lineYMax - lineYMin) * positionPercentage);

        lineGuide.transform.localPosition = new Vector2(lineGuide.transform.localPosition.x, lineYPos);

        correctYMask = yStart + (Mathf.Abs(yEnd - yStart) * positionPercentage);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject.name == gameObject.name)
            {
                FillLiquid();
            }
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

    public SingleScore GetCurrentScore()
    {
        float curY = spriteMask.transform.localPosition.y;
        //Debug.Log("curY: " + curY.ToString());
        //Debug.Log("correctY: " + correctYMask);

        float yChange = Mathf.Abs(yStart - curY);
        float correctYChange = Mathf.Abs(yStart - correctYMask);

        int curScoreTotal = 10;
        int errorQuantity = (int)((System.Math.Abs(correctYChange - yChange) / correctYChange)*curScoreTotal);
        int curScore = curScoreTotal - errorQuantity;

        SingleScore myScore = new SingleScore(curScore, curScoreTotal);
        //Debug.Log(curScore);
        return new SingleScore(curScore, curScoreTotal);
    }
}
