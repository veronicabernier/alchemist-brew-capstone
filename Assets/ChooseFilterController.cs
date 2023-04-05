using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChooseFilterController : MonoBehaviour
{
    //1. add reservoir
    //2. choose filter
    //3. blind cycle

    public GameObject filters;
    public GameObject paperFilter;
    public GameObject permanentFilter;

    public TextMeshProUGUI filterPrompt;

    private string correctFilter = "";
    private string prompt = "";
    private bool choseCorrectly = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void GenerateCorrectFilter()
    {
        int i = UnityEngine.Random.Range(0, 2);
        if(i == 0)
        {
            correctFilter = "paper";
            prompt = "Clear coffee";
        }
        else
        {
            correctFilter = "permanent";
            prompt = "Strong coffee";
        }
        filterPrompt.text = prompt;
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
            filters.SetActive(true);
            GenerateCorrectFilter();
        }
        else
        {
            //filter chosen
            if (paperFilter.activeInHierarchy == false)
            {
                //paper chosen
                choseCorrectly = (correctFilter == "paper");
                permanentFilter.SetActive(false);
            }
            else
            {
                //permanent chosen
                choseCorrectly = (correctFilter == "permanent");
                paperFilter.SetActive(false);
            }
            Debug.Log(choseCorrectly);
        }

    }
}
