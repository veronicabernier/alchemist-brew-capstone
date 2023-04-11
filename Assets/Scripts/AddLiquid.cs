using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AddLiquid : MonoBehaviour
{
    public GameObject spriteMask;
    public bool random = true;

    public GameObject lineGuide;

    [ConditionalHide("random", false)]
    public float lineYMin;
    [ConditionalHide("random", false)]
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
        if (random)
        {
            float positionPercentage = UnityEngine.Random.Range(0.10f, 0.90f);
            float lineYPos = lineYMin + (Mathf.Abs(lineYMax - lineYMin) * positionPercentage);

            lineGuide.transform.localPosition = new Vector2(lineGuide.transform.localPosition.x, lineYPos);

            correctYMask = yStart + (Mathf.Abs(yEnd - yStart) * positionPercentage);
        }
        else
        {
            correctYMask = yEnd;
        }
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
        if(ySpeed < 0)
        {
            if (curY > yEnd)
            {
                float newY = Mathf.Max(curY + ySpeed, yEnd);
                spriteMask.transform.localPosition = new Vector3(spriteMask.transform.localPosition.x, newY);
            }
        }
        else
        {
            if (curY < yEnd)
            {
                float newY = Mathf.Min(curY + ySpeed, yEnd);
                spriteMask.transform.localPosition = new Vector3(spriteMask.transform.localPosition.x, newY);
            }
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
        int curScore = Math.Max(curScoreTotal - errorQuantity, 0);

        List<string> comments = new List<string>();

        if(curScore != curScoreTotal)
        {
            if((correctYChange > 0 && yChange < correctYChange) || (correctYChange < 0 && yChange > correctYChange))
            {
                comments.Add("More water needed");
            }
            else
            {
                comments.Add("Too much water");
            }
        }

        SingleScore myScore = new SingleScore(curScore, curScoreTotal, comments);
        //Debug.Log(curScore);
        return myScore;
    }
}
