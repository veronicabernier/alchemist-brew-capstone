using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecomposeManager : MonoBehaviour
{
    public float decomposeRate = 0.1f;

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
            Debug.Log("Done!");
        }
    }
}
