using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using Michsky.MUIP;

public class LoginScript : MonoBehaviour
{
    private string inputFieldUsernameName = "username";
    private string inputFieldPasswordName = "password";
    private string loginButton = "login";
    public InputField inputFieldUsername;
    public InputField inputFieldPassword;
    public string afterLoginSceneName;

    public NotificationManager popup;
    public NotificationManager popupSuccess;
    public GameObject showPasswordButton;
    public GameObject hidePasswordButton;

    private string username;
    private string password;

    void Start()
    {
        inputFieldUsername = GameObject.FindGameObjectWithTag("username").GetComponent<InputField>();
        inputFieldPassword = GameObject.FindGameObjectWithTag("password").GetComponent<InputField>();
        inputFieldUsername.onValueChanged.AddListener(OnUsernameChanged);
        inputFieldPassword.onValueChanged.AddListener(OnPasswordChanged);

        inputFieldPassword.inputType = InputField.InputType.Password;
    }

    void OnUsernameChanged(string newValue) 
    {
        username = newValue;
        Debug.Log("username input: " + username);
    }

    void OnPasswordChanged(string newValue)
    {
        password = newValue;
    }
    public void OnLoginPress()
    
        {
            WWWForm form = new WWWForm();
            form.AddField("email", username);
            form.AddField("password", password);

            StartCoroutine(PostRequest(form));

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





/*{
    public InputField InputFieldusername;
    public InputField InputFieldpassword;

    private void Start()
    {
        string username = InputFieldusername.text;
        string password = InputFieldpassword.text;

        Debug.Log("username input: " + username);
        Debug.Log("password input: " + password);
    }
}*/



/*{
    public InputField InputFieldusername;
    public InputField InputFieldpassword;

    private string username;
    private string password;

    private void Start()
    {
        // Subscribe to the OnValueChanged event of each input field
        InputFieldusername.onValueChanged.AddListener(OnUsernameChanged);
        InputFieldpassword.onValueChanged.AddListener(OnPasswordChanged);
        Debug.Log("usernameInput = " + InputFieldusername);
        Debug.Log("passwordInput = " + InputFieldpassword);
    }

    private void OnUsernameChanged(string newUsername)
    {
        username = newUsername;
        Debug.Log("New username: " + username);
    }

    private void OnPasswordChanged(string newPassword)
    {
        password = newPassword;
        Debug.Log("New password: " + password);
    }
}*/

//{
// private InputField InputFieldUsername;
// private InputField InputfieldPassword;


// Start is called before the first frame update
//  void Start()
// {
//usernameInput = GameObject.Find("InputFieldUsername").GetComponent<InputField>();
// passwordInput = GameObject.Find("InputfieldPassword").GetComponent<InputField>();
//  Debug.Log("Username input found: " + (InputFieldUsername != null));
//    Debug.Log("Password input found: " + (InputfieldPassword != null));
// }

// Update is called once per frame
// void Update()
//  {
// string username = usernameInput.text;
// string password = passwordInput.text;

// Debug.Log("Input Text 1: " + username);
// Debug.Log("Input Text 2: " + password);
//}
//}
