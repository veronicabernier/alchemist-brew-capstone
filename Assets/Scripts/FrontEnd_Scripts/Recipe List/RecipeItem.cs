using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class RecipeItem : MonoBehaviour
{
    public TextMeshProUGUI brandText;
    public TextMeshProUGUI beanTypeText;
    public TextMeshProUGUI brewMethodText;
    public TextMeshProUGUI coffeeWeightText;
    public TextMeshProUGUI grindSettingText;
    public TextMeshProUGUI roastText;

    public void SetRecipe(RecipeData recipe)
    {
        

        brandText.text = "Brand: " + recipe.brand;
        beanTypeText.text = "Bean Type: " + recipe.bean_type;
        brewMethodText.text = "Brew Method: " + recipe.brew_method;
        coffeeWeightText.text = "Coffee Weight: " + recipe.coffee_weight.ToString();
        grindSettingText.text = "Grind Setting: " + recipe.grind_setting.ToString();
        roastText.text = "Roast: " + recipe.roast;
    }
}
