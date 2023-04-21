using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class Logbook_Connection : MonoBehaviour
{
    private string inputFieldBrewMethodName = "brew_method";
    private string inputFieldGrindname = "grind_setting";
    private string inputFieldBrandName = "brand";
    private string inputFieldRoastname = "roast";
    private string Beantypename = "bean_type";
    private string Coffeeweightname = "coffee_weight";
    private string extweightName = "ext_weight";
    private string Tagsname = "tags";
    private string NotesName = "Notes";
    private string Submitbutton = "submit";

    public InputField inputFieldBrewMethod;
    public InputField inputFieldGrind;
    public InputField inputFieldbrand;
    public InputField inputFieldroast;
    public InputField inputFieldbeanType;
    public InputField inputFieldcoffeeweight;
    public InputField inputFieldextweight;
    public InputField inputFieldNotes;



    public Button Submit;

    public Dropdown tags;


    private string brewMethod;
    private string grind;
    private string brand;
    private string roast;
    private string beanType;
    private string coffeeweight;
    private string extweight;
    private string notes;

    // Start is called before the first frame update
    void Start()
    {
     


    inputFieldBrewMethod = GameObject.FindGameObjectWithTag(inputFieldBrewMethodName).GetComponent<InputField>();
    inputFieldGrind = GameObject.FindGameObjectWithTag(inputFieldGrindname).GetComponent<InputField>();
    inputFieldbrand = GameObject.FindGameObjectWithTag(inputFieldBrandName).GetComponent<InputField>();
 
    inputFieldroast = GameObject.FindGameObjectWithTag(inputFieldRoastname).GetComponent<InputField>();
    inputFieldbeanType = GameObject.FindGameObjectWithTag(Beantypename).GetComponent<InputField>();
    inputFieldcoffeeweight = GameObject.FindGameObjectWithTag(Coffeeweightname).GetComponent<InputField>();
    inputFieldextweight = GameObject.FindGameObjectWithTag(extweightName).GetComponent<InputField>();
    inputFieldNotes = GameObject.FindGameObjectWithTag(NotesName).GetComponent<InputField>();

    Submit = GameObject.FindGameObjectWithTag(Submitbutton).GetComponent<Button>();



}


    /*void OnSubmitPress()
    {
        {
            WWWForm form = new WWWForm();
            form.AddField("username", username);
            form.AddField("email", email);
            form.AddField("password", password);
            form.AddField("confirm", confirmPassword); // confirm password
            form.AddField("private_profile", "True"); // no poner en front end
            form.AddField("birth_date", Birthdate);
            form.AddField("gender", gender.options[gender.value].text);
            form.AddField("location", location); // verificar si se puede coger location 

            StartCoroutine(PostRequest("http://127.0.0.1:5000/signup", form));
        }

        IEnumerator PostRequest(string uri, WWWForm postData)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, postData))
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log("Error: " + webRequest.error);
                }
                else
                {
                    Debug.Log(webRequest.downloadHandler.text);
                }
            }
        }



      
    }
}*/
// Update is called once per frame
void Update()
    {
        
    }
}
