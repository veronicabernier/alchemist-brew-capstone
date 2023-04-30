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
    public GameObject showPasswordButton;
    public GameObject hidePasswordButton;
    public GameObject showConfirmPasswordButton;
    public GameObject hideConfirmPasswordButton;

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

        inputFieldPassword.inputType = InputField.InputType.Password;
        inputFieldConfirmPassword.inputType = InputField.InputType.Password;
    }
    public void GenderValueChangedHappened(Dropdown sender)
    {
        Debug.Log(" You have selected  " + gender.options[gender.value].text); 
    }


    void OnPasswordConfirmChanged(string newValue)
    {
        confirmPassword = newValue;
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
                    popup.description = webRequest.error;
                    popup.UpdateUI();
                    popup.Open();
                }
                else
                {
                    LoginData loginData = JsonUtility.FromJson<LoginData>(webRequest.downloadHandler.text);

                    if (loginData.userId == -1)
                    {
                        popup.description = loginData.Error; ;
                        popup.UpdateUI();
                        popup.Open();
                    }
                    else
                    {
                        popupSuccess.Open();
                        PostInformation.userid = loginData.userId;
                        yield return new WaitForSecondsRealtime(popupSuccess.timer);
                        new SceneChanger().changeScene(afterLoginSceneName);
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

    public void showPassword(bool show)
    {
        if (show)
        {
            inputFieldPassword.inputType = InputField.InputType.Standard;
            inputFieldPassword.textComponent.text = password;
            showPasswordButton.SetActive(false);
            hidePasswordButton.SetActive(true);
        }
        else
        {
            inputFieldPassword.inputType = InputField.InputType.Password;
            inputFieldPassword.textComponent.text = new string('*', password.Length);
            showPasswordButton.SetActive(true);
            hidePasswordButton.SetActive(false);
        }
    }
    public void showConfirmPassword(bool show)
    {
        if (show)
        {
            inputFieldConfirmPassword.inputType = InputField.InputType.Standard;
            inputFieldConfirmPassword.textComponent.text = confirmPassword;
            showConfirmPasswordButton.SetActive(false);
            hideConfirmPasswordButton.SetActive(true);
        }
        else
        {
            inputFieldConfirmPassword.inputType = InputField.InputType.Password;
            inputFieldConfirmPassword.textComponent.text = new string('*', confirmPassword.Length);
            showConfirmPasswordButton.SetActive(true);
            hideConfirmPasswordButton.SetActive(false);
        }
    }
}
