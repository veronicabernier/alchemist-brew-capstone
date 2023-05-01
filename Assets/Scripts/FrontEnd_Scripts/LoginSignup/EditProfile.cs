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

public class EditProfile : MonoBehaviour
{
    public InputField inputFieldUsername;
    public InputField inputFieldEmail;
    public InputField inputFieldLocation;

    public Toggle privateAccount;

    public Bitsplash.DatePicker.DatePickerContent datepickerBirthDate;
    public Bitsplash.DatePicker.DatePickerDropDownBase datepickerDropdown;

    public InputField inputFieldOTP;

    public Dropdown gender;
    //public GameObject popup;
    public NotificationManager popup;
    public NotificationManager popupSuccess;

    public string afterSceneName;

    public GameObject isVerifiedText;
    public GameObject verifyButton;
    public GameObject verifyPanel;

    private string username = "";
    private string email = "";
    private string Birthdate = "";
    private string location = "";

    private ProfileInfo profileInfo = PostInformation.ProfileInfo;

    void Start()
    {
        if (profileInfo == null)
        {
            Debug.LogError("No profile selected!");
            return;
        }
        else if (profileInfo != null)
        {
            inputFieldUsername.text = profileInfo.username;
            inputFieldEmail.text = profileInfo.email;
            inputFieldLocation.text = profileInfo.location;
            privateAccount.isOn = profileInfo.private_profile;

            for(int i = 0; i < gender.options.Count; i++)
            {
                if(gender.options[i].text == profileInfo.gender)
                {
                    gender.value = i;
                }
            }

            Birthdate = profileInfo.birth_date;
            //datepickerBirthDate.startDate = DateTime.Parse(profileInfo.birth_date);
            datepickerBirthDate.endDate = DateTime.Parse(profileInfo.birth_date);
            datepickerDropdown.SelectionChanged();
            //datepickerBirthDate.startDate = new DateTime(1960, 1, 1);
            //datepickerBirthDate.endDate = new DateTime(2030, 12, 31);

            username = inputFieldUsername.text;
            email = inputFieldEmail.text;
            location = inputFieldLocation.text;

            if (profileInfo.confirmation)
            {
                isVerifiedText.SetActive(true);
            }
            else
            {
                verifyButton.SetActive(true);
            }
        }


        inputFieldUsername.onValueChanged.AddListener(OnUsernameChanged);
        inputFieldEmail.onValueChanged.AddListener(OnEmailChange);
        inputFieldLocation.onValueChanged.AddListener(OnLocationChange);

        datepickerBirthDate.OnSelectionChanged.AddListener(OnBirthDateChange);
        gender.onValueChanged.AddListener(delegate
        {
            GenderValueChangedHappened(gender);
        });

    }

    public void OpenOTPConfirmation()
    {
        verifyPanel.SetActive(true);
    }

    public void CloseOTPConfirmation()
    {
        verifyPanel.SetActive(false);
    }

    public void OnSubmitOTP()
    {
        if (inputFieldOTP.text != "")
        {
            WWWForm form = new WWWForm();
            form.AddField("confirmation_code", inputFieldOTP.text);

            StartCoroutine(PostRequest(PostInformation.address + "/signup/" + PostInformation.userid + "/verify", form));
        }
        else
        {
            popup.description = "No code written.";
            popup.UpdateUI();
            popup.Open();
        }

        IEnumerator PostRequest(string uri, WWWForm postData)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, postData))
            {
                webRequest.method = "POST";
                yield return webRequest.SendWebRequest();
                if (webRequest.result != UnityWebRequest.Result.Success || webRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    popup.description = webRequest.error;
                    popup.UpdateUI();
                    popup.Open();
                }
                else
                {
                    if(webRequest.downloadHandler.text.Contains("Wrong code"))
                    {
                        popup.description = "Wrong";
                        popup.UpdateUI();
                        popup.Open();
                    }
                    else
                    {
                        popupSuccess.title = "Verified Email!";
                        popupSuccess.UpdateUI();
                        popupSuccess.Open();
                        yield return new WaitForSecondsRealtime(popupSuccess.timer);
                        new SceneChanger().changeScene(afterSceneName);
                    }
                }
            }
        }
    }

    public void OnResendOTP()
    {
        WWWForm form = new WWWForm();

        StartCoroutine(PostRequest(PostInformation.address + "/signup/" + PostInformation.userid + "/verify/resend", form));


        IEnumerator PostRequest(string uri, WWWForm postData)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, postData))
            {
                webRequest.method = "POST";
                yield return webRequest.SendWebRequest();
                if (webRequest.result != UnityWebRequest.Result.Success || webRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    popup.description = webRequest.error;
                    popup.UpdateUI();
                    popup.Open();
                }
                else
                {
                    popupSuccess.title = "Resent Code.";
                    popupSuccess.UpdateUI();
                    popupSuccess.Open();  
                }
            }
        }
    }

    public void GenderValueChangedHappened(Dropdown sender)
    {
        Debug.Log(" You have selected  " + gender.options[gender.value].text);
    }

    void OnEmailChange(string newvalue)
    {
        email = newvalue;
    }

    void OnBirthDateChange()
    {
        if (datepickerDropdown.GetSelectedDate() != null)
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

    public void OnSavePress()
    {
        string validFieldsError = validFields();
        if (validFieldsError == "")
        {
            WWWForm form = new WWWForm();
            form.AddField("username", username);
            form.AddField("email", email);
            form.AddField("private_profile", privateAccount.isOn.ToString()); // no poner en front end
            form.AddField("birth_date", Birthdate);
            form.AddField("gender", gender.options[gender.value].text);
            form.AddField("location", location); // verificar si se puede coger location 

            StartCoroutine(PostRequest("http://127.0.0.1:5000/" + PostInformation.userid + "/profile/edit", form));
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
                webRequest.method = "PUT";
                yield return webRequest.SendWebRequest();
                if (webRequest.result != UnityWebRequest.Result.Success || webRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    popup.description = webRequest.error;
                    popup.UpdateUI();
                    popup.Open();
                }
                else
                {
                    popupSuccess.title = "Changes Saved!";
                    popupSuccess.UpdateUI();
                    popupSuccess.Open();
                    yield return new WaitForSecondsRealtime(popupSuccess.timer);
                    new SceneChanger().changeScene(afterSceneName);
                }
            }
        }
    }

    private string validFields()
    {
        if (username == "" || email == "" || Birthdate == "" || location == "" || gender.options[gender.value].text == "")
        {
            return "Complete all fields.";
        }

        return "";
    }
}