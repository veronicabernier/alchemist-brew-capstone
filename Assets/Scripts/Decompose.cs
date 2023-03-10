using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decompose : MonoBehaviour
{

    private DecomposeManager dm;
    private Color color;

    // Start is called before the first frame update
    void Start()
    {
        dm = GetComponentInParent<DecomposeManager>();
        color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Distributer")
        {
            //Debug.Log("triggered");
            DecreaseOpacity();
            if (color.a <= 0)
            {
                dm.textureDecomposed();
                //Debug.Log("finished texture");
                gameObject.SetActive(false);
            }
        }

    }

    private void DecreaseOpacity()
    {
        color = new Color(color.r, color.g, color.b, color.a - dm.decomposeRate);
        gameObject.GetComponent<SpriteRenderer>().color = color;
    }

}
