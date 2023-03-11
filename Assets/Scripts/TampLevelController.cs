using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TampLevelController : MonoBehaviour
{
    public GameObject tamper;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ObjectPlaced()
    {
        tamper.transform.GetComponent<Press>().pressable = true;
    }
}
