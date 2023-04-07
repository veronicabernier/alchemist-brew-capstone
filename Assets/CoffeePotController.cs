using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeePotController : MonoBehaviour
{
    public MovementBar mb;

    private void OnMouseDrag()
    {
        mb.BarActions();
    }

    private void OnMouseDown()
    {
        mb.interacting = true;
    }

    private void OnMouseUp()
    {
        mb.interacting = false;
    }
}
