using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetinputExample : MonoBehaviour
{
    public string inputFieldUsernameName = "InputFieldUsername";
    public string inputFieldPasswordName = "InputFieldPassword";
    public InputField inputFieldUsername;
    public InputField inputFieldPassword;

    private string username;
    private string password;

    void Start()
    {
        inputFieldUsername = GameObject.Find(inputFieldUsernameName).GetComponent<InputField>();
        inputFieldPassword = GameObject.Find(inputFieldPasswordName).GetComponent<InputField>();
        inputFieldUsername.onValueChanged.AddListener(OnUsernameChanged);
        inputFieldPassword.onValueChanged.AddListener(OnPasswordChanged);
        Debug.Log("username input: " + username);
        Debug.Log("password input: " + password);
    }

    void OnUsernameChanged(string newValue)
    {
        username = newValue;
    }

    void OnPasswordChanged(string newValue)
    {
        password = newValue;
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
