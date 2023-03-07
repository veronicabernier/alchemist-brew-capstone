using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReservoirController : MonoBehaviour
{
    public AddLiquid al;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LevelFinished()
    {
        SingleScore myScore = al.GetCurrentScore();

        this.SendMessageUpwards("StopLevel", myScore);
    }
}
