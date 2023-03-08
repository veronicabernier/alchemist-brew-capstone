using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapIntoPlace : MonoBehaviour
{
    public Transform snapPoint;
    public float snapRange = 0.5f;
    public bool allowDragAfterSnap = false;
    [Tooltip("Default is messages other script in object when done")]
    public bool messageUpwards = false;

    bool snapIsActive = true;
    private Vector2 ogPosition;

    private void Start()
    {
        ogPosition = transform.position;
    }

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
                if (messageUpwards)
                {
                    this.SendMessageUpwards("ObjectPlaced");
                }
                else
                {
                    this.SendMessage("ObjectPlaced");
                }
            }
        }
        else
        {
            transform.position = ogPosition;
            this.GetComponent<Drag>().RevertToOgSize();
        }
    }
}
