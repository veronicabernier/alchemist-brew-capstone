using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{

    Vector3 mousePositionOffset;
    public bool dragIsActive = true;
    public bool changeSizeWhileDrag = false;
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
            mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
            if (changeSizeWhileDrag)
            {
                transform.localScale = ogSize * sizeChangeFactor;
            }
        }
    }

    private void OnMouseDrag()
    {
        if(dragIsActive)
        {
            transform.position = GetMouseWorldPosition() + mousePositionOffset;
        }
    }

    public void RevertToOgSize()
    {
        if (changeSizeWhileDrag)
        {
            transform.localScale = ogSize;
        }
    }

    private void OnMouseUp()
    {
        if (changeSizeWhileDrag && !snapIntoPlaceScript)
        {
            transform.localScale = ogSize;
        }
    }
}

