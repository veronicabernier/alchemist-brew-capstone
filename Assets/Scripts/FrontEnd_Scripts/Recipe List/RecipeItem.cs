using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RecipeItem : MonoBehaviour
{
    private RecipeData recipe;
    public TextMeshProUGUI brandText;
    public TextMeshProUGUI beanTypeText;
    public TextMeshProUGUI brewMethodText;
    public TextMeshProUGUI coffeeWeightText;
    public TextMeshProUGUI grindSettingText;
    public TextMeshProUGUI roastText;
    public Button selected;
    public static RecipeData SelectedRecipe;


    /*public void Onclick()
    {
        RecipeList.selectedRecipe = recipe;
    }*/
    public void SetRecipe(RecipeData recipe)
    {
        
        this.recipe = recipe;
        brandText.text = "Brand: " + recipe.brand;
        beanTypeText.text = "Bean Type: " + recipe.bean_type;
        brewMethodText.text = "Brew Method: " + recipe.brew_method;
        coffeeWeightText.text = "Coffee Weight: " + recipe.coffee_weight.ToString();
        grindSettingText.text = "Grind Setting: " + recipe.grind_setting.ToString();
        roastText.text = "Roast: " + recipe.roast;

        /*Button button = GetComponent<Button>();*/
        selected.onClick.AddListener(() =>
        {
            // Log the recipeId when the recipe item is clicked
            Debug.Log("Selected recipe: " + recipe.recipeid);
            SelectedRecipe = recipe;
            SceneManager.LoadScene("EditRecipe");

            // Add your code to switch to the new scene and prefilled the information here
            // ...
        });
    }
    

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerPrefs.SetString("SelectedRecipeBrand", recipe.brand);
        PlayerPrefs.SetString("SelectedRecipeRoast", recipe.roast);

        SceneManager.LoadScene("EditRecipe");
    }
    

   /* public void OnClick()
    {
        SceneManager.LoadScene("EditRecipe");
        PlayerPrefs.SetString("SelectedRecipeBrand", recipe.brand);
        PlayerPrefs.SetString("SelectedRecipeBeanType", recipe.bean_type);
        PlayerPrefs.SetString("SelectedRecipeBrewMethod", recipe.brew_method);
        PlayerPrefs.SetString("SelectedRecipeRoast", recipe.roast);
        PlayerPrefs.SetFloat("SelectedRecipeWeight", recipe.coffee_weight);
        PlayerPrefs.SetInt("SelectedRecipeGrindSetting", recipe.grind_setting);
    }*/
}
