using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;
using Michsky.MUIP;

public class AddRecipe : MonoBehaviour
{
    public InputField brandInputField;
    public InputField roastInputField;
    public InputField BeantypeInputField;
    public InputField brewMethodInputField;
    public InputField coffeeeWeightInputField;
    public InputField grindSettingInputField;

    public NotificationManager popup;
    public NotificationManager popupSuccess;
    public string afterSceneName;

    private string brand = "";
    private string roast = "";
    private string beanType = "";
    private string brewMethod = "";
    private string coffeeWeight = "";
    private string grindSetting = "";

    // Start is called before the first frame update
    void Start()
    {
        brandInputField.onValueChanged.AddListener(onBrandInputChange);
        roastInputField.onValueChanged.AddListener(onRoastchange);
        BeantypeInputField.onValueChanged.AddListener(onBeantpChange);
        brewMethodInputField.onValueChanged.AddListener(onbrewmethchange);
        coffeeeWeightInputField.onValueChanged.AddListener(oncoffeeweightchange);
        grindSettingInputField.onValueChanged.AddListener(onGrindchange);;
    }



    public void onSubmit()
    {
        string validFieldsError = validFields();
        if (validFieldsError == "")
        {
            WWWForm form = new WWWForm();
            form.AddField("brand", brand);
            form.AddField("roast", roast);
            form.AddField("bean_type", beanType);
            form.AddField("brew_method", brewMethod);
            form.AddField("coffee_weight", coffeeWeight);
            form.AddField("grind_setting", grindSetting);

            StartCoroutine(PostRequest(form));
        }
        else
        {
            popup.description = validFieldsError;
            popup.UpdateUI();
            popup.Open();
        }

        IEnumerator PostRequest(WWWForm form)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Post(PostInformation.address + PostInformation.userid + "/recipe%20list/add", form))

            {
                webRequest.method = "POST";
                yield return webRequest.SendWebRequest();
                if (webRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    popup.description = webRequest.error;
                    popup.UpdateUI();
                    popup.Open();
                }
                else
                {
                    popupSuccess.description = "";
                    popupSuccess.UpdateUI();
                    popupSuccess.Open();

                    yield return new WaitForSecondsRealtime(popupSuccess.timer);
                    new SceneChanger().changeScene(afterSceneName);
                }
            }
        }



    }


    void onBrandInputChange(string newvalue)
    {
        brand = newvalue;
    }

    void onRoastchange(string newvalue)
    {
        roast = newvalue;
    }

    void onBeantpChange(string newvalue)
    {
        beanType = newvalue;
    }

    void onbrewmethchange(string newvalue)
    {
        brewMethod = newvalue;
    }
    void oncoffeeweightchange(string newvalue)
    {
        coffeeWeight = newvalue;
    }

    void onGrindchange(string newvalue)
    {
        grindSetting = newvalue;
    }

    private string validFields()
    {
        if (brand == "" || roast == "" || beanType == "" || brewMethod == "" || coffeeWeight == "" || grindSetting == "")
        {
            return "Complete all fields.";
        }

        return "";
    }
}