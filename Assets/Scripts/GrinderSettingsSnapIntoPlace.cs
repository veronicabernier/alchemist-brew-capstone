using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrinderSettingsSnapIntoPlace : MonoBehaviour
{
    public Transform[] snapPoints;
    public float snapRange = 0.5f;
    public bool allowDragAfterSnap = false;

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
        foreach (Transform snapPoint in snapPoints)
        {
            float currentDistance = Vector2.Distance(transform.position, snapPoint.position);
            if (currentDistance <= snapRange)
            {
                transform.position = snapPoint.position;
                if (!allowDragAfterSnap)
                {
                    this.GetComponent<Drag>().dragIsActive = false;
                    snapIsActive = false;
                }
            }
            else
            {
                transform.position = ogPosition;
                this.GetComponent<Drag>().RevertToOgSize();
            }
        }
    }
}
