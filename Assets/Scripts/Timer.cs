using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [Tooltip("In Seconds")]
    public int timeToFill = 5;
    public Image progressAmount;
    public TextMeshProUGUI secondsLeft;

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
        secondsLeft.text = Mathf.Max((timeToFill - seconds), 0).ToString();
        if (seconds <= timeToFill)
        {
            progressAmount.fillAmount = timer / timeToFill;
        }
        else
        {
            SendMessageUpwards("LevelFinished");
        }
    }
}
