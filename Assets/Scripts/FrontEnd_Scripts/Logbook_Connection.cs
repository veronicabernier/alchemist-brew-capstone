using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Michsky.MUIP;

public class Logbook_Connection : MonoBehaviour
{
    public InputField brewMethodInputField;
    public InputField grindSettingInputField;
    public InputField brandInputField;
    public InputField roastInputField;
    public InputField BeantypeInputField;
    public InputField coffeeeWeightInputField;
    public InputField exitWeightInputField;
    public InputField notesInputField;

    public Dropdown tags;

    public NotificationManager popup;
    public NotificationManager popupSuccess;
    public string afterSceneName;
    public GameObject stopWatchPanel;
    public Timer_log timer;

    private string brewMethod = "";
    private string grindSetting = "";
    private string brand = "";
    private string roast = "";
    private string beanType = "";
    private string coffeeWeight = "";
    private string tagId = "";
    private string exitWeight = "";
    private string notes = "";

    private string exitTime = "0";

    private RecipeData selectedRecipe = RecipeItem.SelectedRecipe;

    // Start is called before the first frame update
    void Start()
    {
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
            coffeeeWeightInputField.text = selectedRecipe.coffee_weight.ToString();
            grindSettingInputField.text = selectedRecipe.grind_setting.ToString();

            brand = brandInputField.text;
            roast = roastInputField.text;
            beanType = BeantypeInputField.text;
            brewMethod = brewMethodInputField.text;
            coffeeWeight = coffeeeWeightInputField.text;
            grindSetting = grindSettingInputField.text;
        }

        brandInputField.onValueChanged.AddListener(onBrandInputChange);
        roastInputField.onValueChanged.AddListener(onRoastchange);
        BeantypeInputField.onValueChanged.AddListener(onBeantpChange);
        brewMethodInputField.onValueChanged.AddListener(onbrewmethchange);
        coffeeeWeightInputField.onValueChanged.AddListener(oncoffeeweightchange);
        grindSettingInputField.onValueChanged.AddListener(onGrindchange);

        exitWeightInputField.onValueChanged.AddListener(onExitWeightChange);
        notesInputField.onValueChanged.AddListener(onNoteschange);
        tags.onValueChanged.AddListener( delegate { onTagchange(); });
    }

    public void OpenTimer()
    {
        stopWatchPanel.SetActive(true);
    }

    public void closeTimer()
    {
        stopWatchPanel.SetActive(false);
        exitTime = ((int) timer.val).ToString();
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

            form.AddField("ext_time", exitTime);
            form.AddField("ext_weight", exitWeight);
            form.AddField("notes", notes);
            form.AddField("tagid", tagId);

            Debug.Log(form.ToString());
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
            using (UnityWebRequest webRequest = UnityWebRequest.Post("http://127.0.0.1:5000/" + PostInformation.userid + "/"+ selectedRecipe.recipeid + "/add", form))

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

    void onExitWeightChange(string newvalue)
    {
        exitWeight = newvalue;
    }

    void onNoteschange(string newvalue)
    {
        notes = newvalue;
    }
    void onTagchange()
    {
        tagId = tags.value.ToString();
    }

    private string validFields()
    {
        if (brand == "" || roast == "" || beanType == "" || brewMethod == "" || coffeeWeight == "" || grindSetting == "" || tagId == "" || exitWeight == "" || notes == "")
        {
            return "Complete all fields.";
        }

        return "";
    }

}

