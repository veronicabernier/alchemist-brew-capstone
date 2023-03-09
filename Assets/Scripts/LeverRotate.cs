using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeverRotate : MonoBehaviour
{
    public enum LeverPositions
    {
        Down,
        MiddleFront,
        Up,
        MiddleBack
    }

    [LabeledArrayAttribute(typeof(LeverPositions))]
    public GameObject[] leverPositions = new GameObject[4];
    //0: down, 1: middle front, 2: up, 3: middle back
    int curObject = 0;
    public float yOffset;

    private Vector3 mousePositionOffset;
    private bool dragging = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (dragging)
        {
            MoveLever();
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDown()
    {
        mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
    }

    private void OnMouseUp()
    {
        dragging = false;
    }

    private void OnMouseDrag()
    {
        dragging = true;
        //MoveLever();

    }

    private void MoveLever()
    {
        Vector3 curOffset = mousePositionOffset - (gameObject.transform.position - GetMouseWorldPosition());
        //Debug.Log(curOffset);
        if (curObject == 0)
        {
            //is down --> goes middle front
            if (curOffset.y > yOffset / 2)
            {
                //middle front
                leverPositions[curObject].SetActive(false);
                curObject += 1;
                leverPositions[curObject].SetActive(true);
            }
        }
        else if (curObject == 1)
        {
            //is middle front --> goes up
            if (curOffset.y > yOffset)
            {
                //up
                leverPositions[curObject].SetActive(false);
                curObject += 1;
                leverPositions[curObject].SetActive(true);
            }
        }
        else if (curObject == 2)
        {
            //is up --> goes middle back
            if (curOffset.y < -yOffset / 2)
            {
                //middle back
                leverPositions[curObject].SetActive(false);
                curObject += 1;
                leverPositions[curObject].SetActive(true);
            }
        }
        else if (curObject == 3)
        {
            //is middle back --> goes down
            if (curOffset.y < -yOffset)
            {
                //down
                leverPositions[curObject].SetActive(false);
                curObject = 0;
                leverPositions[0].SetActive(true);
            }
        }
    }
}
