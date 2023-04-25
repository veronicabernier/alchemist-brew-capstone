using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.Linq;
/*using JSonhelper;*/

public class RecipeList : MonoBehaviour
{
    public GameObject recipeItemPrefab;
    /*public Transform contentTransform;*/
    public RectTransform contentTransform;
    public Dropdown dropdownSearchable;
    private RecipeData[] recipes;
    private List<string> brands = new List<string>();


    void Start()
    {
        StartCoroutine(GetRecipes());
        /*dropdownSearchable.onValueChanged.AddListener(OnDropdownValueChanged);*/
    }
   /* void OnDropdownValueChanged(int index)
    {
        
        string selectedValue = dropdownSearchable.options[index].text;

        // Filter the brews based on the selected value
        RecipeData[] filteredRecipies = recipes.Where(recipes => recipes.brand == selectedValue).ToArray();

        // Update the list with the filtered brews
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        foreach (RecipeData i in filteredRecipies)
        {
            GameObject recipeItem = Instantiate(recipeItemPrefab, contentTransform);
            recipeItem.GetComponent<RecipeItem>().SetRecipe(i);
        }
    }*/

    IEnumerator GetRecipes()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("http://127.0.0.1:5000/1/recipe%20list"))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error retrieving recipe data: " + webRequest.error);
            }
            else
            {
                string recipeData = webRequest.downloadHandler.text;
                //recipeData = "{\r\n  \"Recepies\": [\r\n    {\r\n      \"bean_type\": \"arabica\",\r\n      \"brand\": \"Don Luis\",\r\n      \"brew_method\": \"espresso\",\r\n      \"coffee_weight\": 11.5,\r\n      \"grind_setting\": 1.0,\r\n      \"recipe_visibility\": true,\r\n      \"recipeid\": 14,\r\n      \"roast\": \"dark\",\r\n      \"userid\": 1\r\n    },\r\n    {\r\n      \"bean_type\": \"arabica\",\r\n      \"brand\": \"Don Luis\",\r\n      \"brew_method\": \"Moka Pot\",\r\n      \"coffee_weight\": 20.0,\r\n      \"grind_setting\": 4.0,\r\n      \"recipe_visibility\": true,\r\n      \"recipeid\": 16,\r\n      \"roast\": \"dark\",\r\n      \"userid\": 1\r\n    }\r\n]\r\n}";
                Debug.Log("Recipe data: " + recipeData);
                recipes = JSonhelper.FromJson<RecipeData>(recipeData);
              
                
                if (recipes == null)
                {
                    Debug.Log("Recipes array is null.");
                }
                else if (recipes.Length == 0)
                {
                    Debug.Log("Recipes array is empty.");
                }

                Debug.Log("Roast: " + recipes[2].roast);



                 foreach (RecipeData i in recipes)
                /*for (int i = 0; i < recipes.length; i++)*/
                 {
                    
                     
                     
                    GameObject recipeItem = Instantiate(recipeItemPrefab, contentTransform);
                    recipeItem.GetComponent<RecipeItem>().SetRecipe(i);
                }
                GameObject firstRecipeItem = contentTransform.GetChild(0).gameObject;
                Destroy(firstRecipeItem);
                float newHeight = 250 * recipes.Length;
                contentTransform.sizeDelta = new Vector2(contentTransform.sizeDelta.x, newHeight);
                contentTransform.anchoredPosition = new Vector2(contentTransform.anchoredPosition.x, 0);
                Debug.Log("Successfully parsed " + recipes.Length + " recipes.");
                Debug.Log(recipes[1].brand + "recipe brand");

            }
            for (int i = 0; i < recipes.Length; i++)
            {
                if (!brands.Contains(recipes[i].brand))
                {
                    brands.Add(recipes[i].brand);
                }
            }
            brands.Sort();

            dropdownSearchable.ClearOptions();
            dropdownSearchable.AddOptions(brands);

           
        }
    }
}

[System.Serializable]
public class RecipeData
{
    public string bean_type;
    public string brand;
    public string brew_method;
    public float coffee_weight;
    public int grind_setting;
    public bool recipe_visibility;
    public int recipeid;
    public string roast;
    public int userid;
}
