/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class DisplayRecipes : MonoBehaviour
{
    public GameObject recipePrefab; // Reference to the prefab for displaying recipe information
    public Transform recipeContainer; // Reference to the transform that will contain the instantiated prefabs

    private string url = "http://your-flask-server.com/recipes"; // URL of the Flask route that returns recipe data

    [System.Serializable]
    private class RecipeData
    {
        public Recipe1[] Recepies;
    }

    [System.Serializable]
    private class Recipe1
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


    public void Display()
    {
        StartCoroutine(GetRecipeData());
    }

    private IEnumerator GetRecipeData()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Send the request and wait for the response
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                // Handle errors
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                // Parse the response JSON string into a RecipeData object using JsonUtility
                RecipeData recipeData = JsonUtility.FromJson<RecipeData>("{\"Recepies\":" + webRequest.downloadHandler.text + "}");

                // Loop through each recipe in the RecipeData object and display the information using prefabs
                foreach (var recipe in recipeData.Recepies)
                {
                    // Create a new instance of the recipe prefab
                    var recipeInstance = Instantiate(recipePrefab, recipeContainer);

                    // Set the text of the UI elements in the prefab to the corresponding recipe values
                    recipeInstance.GetComponentInChildren<TextMeshProUGUI>().SetText("Bean Type: " + recipe.bean_type);
                    recipeInstance.GetComponentInChildren<TextMeshProUGUI>().SetText("Brand: " + recipe.brand);
                    recipeInstance.GetComponentInChildren<TextMeshProUGUI>().SetText("Brew Method: " + recipe.brew_method);
                    recipeInstance.GetComponentInChildren<TextMeshProUGUI>().SetText("Coffee Weight: " + recipe.coffee_weight);
                    recipeInstance.GetComponentInChildren<TextMeshProUGUI>().SetText("Grind Setting: " + recipe.grind_setting);
                    recipeInstance.GetComponentInChildren<TextMeshProUGUI>().SetText("Roast: " + recipe.roast);
                    recipeInstance.GetComponentInChildren<TextMeshProUGUI>().SetText("Recipe ID: " + recipe.recipeid);
                }
            }
        }
    }
}
*/