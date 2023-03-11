using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Press : MonoBehaviour
{
    public float yMovement;
    public float xMovement;

    public SpriteRenderer texture;
    public int pressesRequired = 3;

    [System.NonSerialized]
    public bool pressable = false;

    private int curPress = 0;

    private void OnMouseDown()
    {
        if (pressable)
        {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + xMovement, gameObject.transform.localPosition.y + yMovement, gameObject.transform.localPosition.z);
            curPress += 1;
            texture.color = new Color(texture.color.r, texture.color.g, texture.color.b, 1f - 1f*((float) curPress/(float) pressesRequired));
        }
    }
        

    private void OnMouseUp()
    {
        if (pressable)
        {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - xMovement, gameObject.transform.localPosition.y - yMovement, gameObject.transform.localPosition.z);
            if (curPress == pressesRequired)
            {
                texture.enabled = false;
                SendMessageUpwards("PressesDone");
            }
        }
        
    }
}
