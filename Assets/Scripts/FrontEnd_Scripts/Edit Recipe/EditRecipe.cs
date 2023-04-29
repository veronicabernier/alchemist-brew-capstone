using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;


public class EditRecipe : MonoBehaviour
{
    public InputField brandInputField;
    public InputField roastInputField;
    public InputField BeantypeInputField;
    public InputField brewMethodInputField;
    public InputField coffeeeMethodInputField;
    public InputField grindSettingInputField;
    public TextMeshProUGUI successMessage;
    public float messageduration = 3f;

    public Button Submit;



     private RecipeData selectedRecipe = RecipeItem.SelectedRecipe;

    // Start is called before the first frame update
    void Start()
    {
        /*RecipeData selectedRecipe = RecipeItem.SelectedRecipe;*/
        if (selectedRecipe == null)
        {
            Debug.LogError("No recipe selected!");
            return;
        }
        else if (selectedRecipe != null)
        {
            brandInputField.text = selectedRecipe.brand;
            roastInputField.text = selectedRecipe.roast;
            BeantypeInputField.text = selectedRecipe.bean_type;
            brewMethodInputField.text = selectedRecipe.brew_method;
            coffeeeMethodInputField.text = selectedRecipe.coffee_weight.ToString();
            grindSettingInputField.text = selectedRecipe.grind_setting.ToString();
        }
      
        brandInputField.onValueChanged.AddListener(onBrandInputChange);
        roastInputField.onValueChanged.AddListener(onRoastchange);
        BeantypeInputField.onValueChanged.AddListener(onBeantpChange);
        brewMethodInputField.onValueChanged.AddListener(onbrewmethchange);
        coffeeeMethodInputField.onValueChanged.AddListener(oncoffeeweightchange);
        grindSettingInputField.onValueChanged.AddListener(onGrindchange);


        Submit.onClick.AddListener(onSubmit);
    }
    void onSubmit()
    {
        {
            WWWForm form = new WWWForm();
            form.AddField("brand", brandInputField.text);
            form.AddField("roast", roastInputField.text);
            form.AddField("bean_type", BeantypeInputField.text);
            form.AddField("brew_method", brewMethodInputField.text);
            form.AddField("coffee_weight", coffeeeMethodInputField.text);
            form.AddField("grind_setting", grindSettingInputField.text);

            StartCoroutine(PostRequest(form));

        }

        IEnumerator PostRequest(WWWForm form)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Post("http://127.0.0.1:5000/1/" + selectedRecipe.recipeid + "/edit", form))
            /*using (UnityWebRequest webRequest = UnityWebRequest.Post("http://127.0.0.1:5000/1/14/edit", form))*/
            {
                webRequest.method = "PUT";
                yield return webRequest.SendWebRequest();
                if (webRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log("Error: " + webRequest.error);
                }
                else
                {
                    Debug.Log(webRequest.downloadHandler.text);
                    successMessage.text = "Recipe Edited Successfully!";

                    yield return new WaitForSeconds(messageduration);
                    successMessage.text = "";

                    SceneManager.LoadScene("Recipe_List");

                    /* Debug.Log("password input: " + password);*/
                }
            }
        }



    }
    

    void onBrandInputChange(string newvalue)
    {
        brandInputField.text = newvalue;
        Debug.Log("username input: " + brandInputField.text);
    }

    void onRoastchange(string newvalue)
    {
        roastInputField.text = newvalue;
    }

    void onBeantpChange(string newvalue)
    {
        BeantypeInputField.text = newvalue;
    }

    void onbrewmethchange(string newvalue)
    {
        brewMethodInputField.text = newvalue;
    }
    void oncoffeeweightchange (string newvalue)
    {
        RecipeData selectedRecipe = RecipeItem.SelectedRecipe;
        coffeeeMethodInputField.text = newvalue;
        selectedRecipe.coffee_weight = int.Parse(coffeeeMethodInputField.text);
        Debug.Log("coffee weight input: " + selectedRecipe.coffee_weight);
    }

    void onGrindchange(string newvalue)
    {
        RecipeData selectedRecipe = RecipeItem.SelectedRecipe;
        grindSettingInputField.text = newvalue;
        selectedRecipe.grind_setting = int.Parse(grindSettingInputField.text);
        Debug.Log("coffee grind input: " + selectedRecipe.grind_setting);
    }


    

// Update is called once per frame
void Update()
    {
        
    }
}

