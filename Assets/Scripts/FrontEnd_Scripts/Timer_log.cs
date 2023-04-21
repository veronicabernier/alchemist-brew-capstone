using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer_log : MonoBehaviour
{
    public TMP_Text Dispvar;
    float val;
    bool str;

    // Start is called before the first frame update
    void Start()
    {
        val = 0;
        str = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (str)
        {
            val += Time.deltaTime;
            
        }
        Dispvar.SetText(val.ToString("F2"));
    }
    public void start()
    {
        str = true;
    }
    public void stop()
    {
        str= false;
    }
    public void reset()
    {
        str = false;
        val = 0;
    }
}
