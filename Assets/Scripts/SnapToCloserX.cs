using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToCloserX : MonoBehaviour
{
    public float[] snapPointsX;
    public float snapRange = 0.5f;
    public bool allowDragAfterSnap = false;

    private bool snapIsActive = true;
    private Vector2 ogPosition;

    private void Start()
    {
        ogPosition = transform.localPosition;
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
        float curMinDistance = float.PositiveInfinity;
        int curMinDistanceIndex = 0;
        for (int i = 0; i < snapPointsX.Length; i++)
        {
            float curDistance = Mathf.Abs(transform.localPosition.x - snapPointsX[i]);

            if (curDistance < curMinDistance)
            {
                curMinDistance = curDistance;
                curMinDistanceIndex = i;
            }
        }
        transform.localPosition = new Vector3(snapPointsX[curMinDistanceIndex], transform.localPosition.y, transform.localPosition.z);
    }

}
