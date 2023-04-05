using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseFilterController : MonoBehaviour
{
    //1. add reservoir
    //2. choose filter
    //3. blind cycle

    public GameObject filters;
    public GameObject paperFilter;
    public GameObject permanentFilter;


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
        if(filters.activeSelf == false)
        {
            //reservoir placed
            Debug.Log("reservoir");
            filters.SetActive(true);
        }
        else
        {
            //filter chosen
            if (paperFilter.activeInHierarchy == false)
            {
                //paper chosen
                Debug.Log("paper");
                permanentFilter.SetActive(false);
            }
            else
            {
                //permanent chosen
                Debug.Log("permanent");
                paperFilter.SetActive(false);
            }
        }

    }
}
