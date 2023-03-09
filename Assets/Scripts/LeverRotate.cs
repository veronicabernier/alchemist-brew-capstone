using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeverRotate : MonoBehaviour
{
    public GameObject[] leverPositions;
    int curObject = 0;
    public float yOffset;

    private Vector3 mousePositionOffset;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDown()
    {
        mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        Vector3 curOffset = mousePositionOffset - (gameObject.transform.position - GetMouseWorldPosition());
        //Debug.Log(curOffset);
        if(curOffset.y > yOffset)
        {
            Debug.Log("up");
        }
    }
}
