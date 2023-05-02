using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;
/*using JSonhelper;*/

public class RecipeList : MonoBehaviour
{
    public GameObject recipeItemPrefab;
    
    /*public Transform contentTransform;*/
    public RectTransform contentTransform;
    private RecipeData[] recipes;
    public GameObject emptyText;
    public GameObject spinner;

    public static RecipeData SelectedRecipe;


    public void LoadRecipeDetails(RecipeData recipe)
    {
        SelectedRecipe = recipe;
        SceneManager.LoadScene("EditRecipe");
    }
    void Start()
    {
        StartCoroutine(GetRecipes());
      
    }

   

    IEnumerator GetRecipes()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(PostInformation.address + PostInformation.userid + "/recipe%20list"))
        {
            spinner.SetActive(true);
            yield return webRequest.SendWebRequest();
            spinner.SetActive(false);

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error retrieving recipe data: " + webRequest.error);
                emptyText.GetComponent<TextMeshProUGUI>().text = webRequest.error;
                emptyText.SetActive(true);
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
                    emptyText.SetActive(true);
                }


                 foreach (RecipeData i in recipes)
                /*for (int i = 0; i < recipes.length; i++)*/
                 {
                    GameObject recipeItem = Instantiate(recipeItemPrefab, contentTransform);
                    recipeItem.GetComponent<RecipeItem>().SetRecipe(i);
                   /* recipeItem.GetComponent<Button>().onClick.AddListener(() => recipeItem.GetComponent<RecipeItem>().OnClickRecipeItem());*/
                }

                GridLayoutGroup glg = contentTransform.GetComponent<GridLayoutGroup>();
                float newHeight = (glg.cellSize.y + glg.padding.top + glg.padding.bottom + glg.spacing.y) * recipes.Length;
                contentTransform.sizeDelta = new Vector2(contentTransform.sizeDelta.x, newHeight);
                contentTransform.anchoredPosition = new Vector2(contentTransform.anchoredPosition.x, 0);
            }    
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
