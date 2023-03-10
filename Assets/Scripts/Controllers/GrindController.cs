using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrindController : MonoBehaviour
{
    public GameObject container;
    public GameObject beansToGrindParent;
    public LeverRotate lc;
    public SnapToCloserX stcx;

    bool stcxDeactivated = false;

    // Start is called before the first frame update
    void Start()
    {
        lc.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!stcxDeactivated && lc.isDirty)
        {
            stcx.DeactivateSnap();
            stcxDeactivated = true;
        }
    }

    public void ObjectPlaced()
    {
        container.SetActive(false);
        beansToGrindParent.SetActive(true);
        lc.enabled = true;
    }
}
