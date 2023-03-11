using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Press : MonoBehaviour
{
    public float yMovement;
    public float xMovement;

    [System.NonSerialized]
    public bool pressable = false;

    private void OnMouseDown()
    {
        if (pressable)
        {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + xMovement, gameObject.transform.localPosition.y + yMovement, gameObject.transform.localPosition.z);
        }
    }
        

    private void OnMouseUp()
    {
        if (pressable)
        {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - xMovement, gameObject.transform.localPosition.y - yMovement, gameObject.transform.localPosition.z);
        }
        
    }
}
