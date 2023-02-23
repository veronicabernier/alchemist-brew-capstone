using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{

    Vector3 mousePositionOffset;
    public bool dragIsActive = true;

    private Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDown()
    {
        if(dragIsActive)
        {
            mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
        }
    }

    private void OnMouseDrag()
    {
        if(dragIsActive)
        {
            transform.position = GetMouseWorldPosition() + mousePositionOffset;
        }
    }
}

