using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapIntoPlace : MonoBehaviour
{
    public bool snapToPoint = true;
    [ConditionalHide("snapToPoint")]
    public Transform snapPoint;
    [ConditionalHide("snapToPoint")]
    public float snapRange = 0.5f;

    [ConditionalHide("snapToPoint")]
    public bool allowDragAfterSnap = false;
    [ConditionalHide("snapToPoint")]
    public bool dissapearAfterSnap = false;
    [Tooltip("Default is messages other script in object when done")]
    [ConditionalHide("snapToPoint")]
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
        if (snapToPoint)
        {
            float currentDistance = Vector2.Distance(transform.position, snapPoint.position);
            if (currentDistance <= snapRange)
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
                    if (dissapearAfterSnap)
                    {
                        this.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                RevertToOriginal();
            }
        }
        else
        {
            RevertToOriginal();
        }

    }

    public void RevertToOriginal()
    {
        transform.position = ogPosition;
        this.GetComponent<Drag>().RevertToOgSize();
    }
}
