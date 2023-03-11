using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{

    Vector3 mousePositionOffset;
    [System.NonSerialized]
    public bool dragIsActive = true;
    public bool onlyHorizontal = false;
    [ConditionalHide("onlyHorizontal", false)]
    public float minXLimit;
    [ConditionalHide("onlyHorizontal", false)]
    public float maxXLimit;

    public bool snapIntoPlaceScript = false;
    public float sizeChangeFactor = 1.0f;

    private Vector3 ogSize;

    private void Start()
    {
        ogSize = transform.localScale;
    }
    private Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDown()
    {
        if(dragIsActive)
        {
            mousePositionOffset = gameObject.transform.localPosition - GetMouseWorldPosition();
            transform.localScale = ogSize * sizeChangeFactor;
        }
    }

    private void OnMouseDrag()
    {
        if(dragIsActive)
        {
            if (onlyHorizontal)
            {
                float newX = (GetMouseWorldPosition() + mousePositionOffset).x;
                newX = Mathf.Max(Mathf.Min(maxXLimit, newX), minXLimit);
                transform.localPosition = new Vector3(newX, transform.localPosition.y, transform.localPosition.z);
            }
            else
            {
                transform.localPosition = GetMouseWorldPosition() + mousePositionOffset;
            }
        }
    }

    public void RevertToOgSize()
    {
        transform.localScale = ogSize;
    }

    private void OnMouseUp()
    {
        if (!snapIntoPlaceScript)
        {
            transform.localScale = ogSize;
        }
    }

}

