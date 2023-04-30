using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;
using Michsky.MUIP;
using static LoginScript;
using System;

public class SignupConnection : MonoBehaviour
{ 
    public InputField inputFieldUsername;
    public InputField inputFieldPassword;
    public InputField inputFieldEmail;
    public InputField inputFieldConfirmPassword;
    public InputField inputFieldLocation;

    public Bitsplash.DatePicker.DatePickerContent datepickerBirthDate;
    public Bitsplash.DatePicker.DatePickerDropDownBase datepickerDropdown;

    public Dropdown gender;
    //public GameObject popup;
    public NotificationManager popup;
    public NotificationManager popupSuccess;
    public string afterLoginSceneName;

    private string username = "";
    private string password = "";
    private string email = "";
    private string confirmPassword = "";
    private string Birthdate = "";
    private string location = "";

    void Start()
    {
        inputFieldUsername.onValueChanged.AddListener(OnUsernameChanged);
        inputFieldPassword.onValueChanged.AddListener(OnPasswordChanged);
        inputFieldEmail.onValueChanged.AddListener(OnEmailChange);
        inputFieldConfirmPassword.onValueChanged.AddListener(OnPasswordConfirmChanged);
        inputFieldLocation.onValueChanged.AddListener(OnLocationChange);

        datepickerBirthDate.OnSelectionChanged.AddListener(OnBirthDateChange);
        gender.onValueChanged.AddListener(delegate
        {
            GenderValueChangedHappened(gender);
        });
    }
    public void GenderValueChangedHappened(Dropdown sender)
    {
        Debug.Log(" You have selected  " + gender.options[gender.value].text); 
    }


    void OnPasswordConfirmChanged(string newvalue)
    {
        confirmPassword = newvalue;
    }
    void OnEmailChange(string newvalue)
    {
        email = newvalue;
    }

    void OnBirthDateChange()
    {
        if(datepickerDropdown.GetSelectedDate() != null)
        {
            Birthdate = DateTime.Parse(datepickerDropdown.GetSelectedDate().ToString()).ToString("yyyy-MM-dd");
        }
    }

    void OnLocationChange(string newValue)
    {
        location = newValue;
    }
    void OnUsernameChanged(string newValue)
    {
        username = newValue;
    }

    void OnPasswordChanged(string newValue)
    {
        password = newValue;
    }

    public void OnSignupPress()
    {
        string validFieldsError = validFields();
        if (validFieldsError == "")
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
        else
        {
            popup.description = validFieldsError;
            popup.UpdateUI();
            popup.Open();
        }

        IEnumerator PostRequest(string uri, WWWForm postData)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, postData))
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log("Error: " + webRequest.error);
                    popup.description = webRequest.error;
                    popup.UpdateUI();
                    popup.Open();
                }
                else
                {
                    Debug.Log(webRequest.downloadHandler.text);

                    if (webRequest.downloadHandler.text.Contains("Password does not match"))
                    {
                        popup.description = "Password doesn't match";
                        popup.UpdateUI();
                        popup.Open();
                    }
                    else
                    {
                        if(webRequest.downloadHandler.text.Contains("Singup complete"))
                        {
                            PostInformation.userid = JsonUtility.FromJson<LoginData>(webRequest.downloadHandler.text).userId;
                            popupSuccess.Open();

                            SceneChanger sc = new SceneChanger();
                            sc.changeScene(afterLoginSceneName);
                        }
                        else
                        {
                            popup.description = webRequest.downloadHandler.text;
                            popup.UpdateUI();
                            popup.Open();
                        }

                    }
                }
            }
        }



        Debug.Log("username input: " + username);
        Debug.Log("password input: " + password);
        Debug.Log("confirmpassword input: " + confirmPassword);
        Debug.Log("email input: " + email);
        Debug.Log("location input: " + location);
        Debug.Log("BirthDate input: " + Birthdate);
    }

    private string validFields()
    {
        if(username == "" ||  password == "" || email == "" || confirmPassword == "" || Birthdate == "" || location == "" || gender.options[gender.value].text == "")
        {
            return "Complete all fields.";
        }

        return "";
    }

}
