using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecomposeManager : MonoBehaviour
{
    public float decomposeRate = 0.1f;
    public SpriteRenderer distributer;

    private int texturesLeft;


    // Start is called before the first frame update
    void Start()
    {
        texturesLeft = GetComponentsInChildren<Decompose>().Length;
    }

    public void textureDecomposed()
    {
        texturesLeft -= 1;
        if (texturesLeft == 0)
        {
            //Debug.Log("Done!");
            distributer.color = new Color(0.55f, 0.55f, 0.55f, 0.8f);
        }
    }
}
