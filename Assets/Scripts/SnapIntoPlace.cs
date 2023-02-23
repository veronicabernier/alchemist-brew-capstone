using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapIntoPlace : MonoBehaviour
{
    public Transform snapPoint;
    public float snapRange = 0.5f;
    public bool allowDragAfterSnap = false;

    bool snapIsActive = true;


    public void OnMouseUp()
    {
        if (snapIsActive)
        {
            OnDragEnded();
        }
    }

    private void OnDragEnded()
    {
        float currentDistance = Vector2.Distance(transform.position, snapPoint.position);
        if(currentDistance <= snapRange)
        {
            transform.position = snapPoint.position;
            if (!allowDragAfterSnap)
            {
                this.GetComponent<Drag>().dragIsActive = false;
                snapIsActive = false;
                this.SendMessage("ObjectPlaced");
            }
        }
    }
}
