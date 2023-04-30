using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using Michsky.MUIP;

public class LoginScript : MonoBehaviour
{
    public InputField inputFieldEmail;
    public InputField inputFieldPassword;
    public string afterLoginSceneName;

    public NotificationManager popup;
    public NotificationManager popupSuccess;
    public GameObject showPasswordButton;
    public GameObject hidePasswordButton;

    private string email = "";
    private string password = "";

    void Start()
    {
        inputFieldEmail.onValueChanged.AddListener(OnEmailChanged);
        inputFieldPassword.onValueChanged.AddListener(OnPasswordChanged);

        inputFieldPassword.inputType = InputField.InputType.Password;
    }

    void OnEmailChanged(string newValue) 
    {
        email = newValue;
    }

    void OnPasswordChanged(string newValue)
    {
        password = newValue;
    }
    public void OnLoginPress()
    {
        string validFieldsError = validFields();
        if (validFieldsError == "")
        {
            WWWForm form = new WWWForm();
            form.AddField("email", email);
            form.AddField("password", password);

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
            using (UnityWebRequest webRequest = UnityWebRequest.Post("http://127.0.0.1:5000/login", form))
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
    }

    private string validFields()
    {
        if (password == "" || email == "")
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


    [System.Serializable]
    public class LoginData
    {
        public string Error;
        public int userId;
    }
}

