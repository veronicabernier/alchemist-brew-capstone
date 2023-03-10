using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [Tooltip("In Seconds")]
    public int timeToFill = 5;
    public Image progressAmount;

    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;
        int seconds = (int)(timer % 60);
        if (seconds <= timeToFill)
        {
            progressAmount.fillAmount = timer / timeToFill;
        }
        else
        {
            SendMessageUpwards("ProgressDone");
        }
    }
}
