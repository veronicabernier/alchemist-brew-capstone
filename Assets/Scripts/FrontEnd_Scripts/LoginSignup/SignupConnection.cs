using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;


public class SignupConnection : MonoBehaviour
{ 
private string inputFieldUsernameName = "username";
private string inputFieldEmailname = "Email";
private string inputFieldPasswordName = "password";
private string inputFieldConfirmPasswordname = "ConfirmPassword";
private string BirthDatename = "BirthDate";
private string Locationname = "Location";
private string SignUpButton = "SignUp";

public TextMeshProUGUI successMessage;
public float messageduration = 3f;


    public InputField inputFieldUsername;
public InputField inputFieldPassword;
public InputField inputFieldEmail;
    public InputField inputFieldConfirmPassword;
    public InputField inputFieldBirthDate;
    public InputField inputFieldLocation;
public Button SignupObject;

public Dropdown gender;

    


private string username;
private string password;
    private string email;
    private string confirmPassword;
    private string Birthdate;
    private string location;

void Start()
{
        inputFieldUsername = GameObject.FindGameObjectWithTag(inputFieldUsernameName).GetComponent<InputField>();
        inputFieldPassword = GameObject.FindGameObjectWithTag(inputFieldPasswordName).GetComponent<InputField>();
        inputFieldConfirmPassword= GameObject.FindGameObjectWithTag(inputFieldConfirmPasswordname).GetComponent<InputField>();
        inputFieldEmail = GameObject.FindGameObjectWithTag(inputFieldEmailname).GetComponent<InputField>();
        inputFieldBirthDate = GameObject.FindGameObjectWithTag(BirthDatename).GetComponent<InputField>();
        inputFieldLocation = GameObject.FindGameObjectWithTag(Locationname).GetComponent<InputField>();
        SignupObject = GameObject.FindGameObjectWithTag(SignUpButton).GetComponent<Button>();







        inputFieldUsername.onValueChanged.AddListener(OnUsernameChanged);
        inputFieldPassword.onValueChanged.AddListener(OnPasswordChanged);
        inputFieldEmail.onValueChanged.AddListener(OnEmailChange);
        inputFieldConfirmPassword.onValueChanged.AddListener(OnPasswordConfirmChanged);
        inputFieldLocation.onValueChanged.AddListener(OnLocationChange);
        inputFieldBirthDate.onValueChanged.AddListener(OnBirthDateChange);








        SignupObject.onClick.AddListener(OnSignupPress);

        gender.onValueChanged.AddListener(delegate
        {
            GenderValueChangedHappened(gender);
        });


}
    public void GenderValueChangedHappened(Dropdown sender)
    {
        Debug.Log(" You have selected  " + gender.options[gender.value].text); 
    }



    /*//private string username;
    private string password;
    private string email;
    private string confirmPassword;
    private string Birthdate;
    private string location;*/

    void OnPasswordConfirmChanged(string newvalue)
    {
        confirmPassword = newvalue;

    }
    void OnEmailChange(string newvalue)
    {
        email = newvalue;

    }

    void OnBirthDateChange(string newValue)
    {
        Birthdate = newValue;
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

  






    void OnSignupPress()
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

                    if (webRequest.downloadHandler.text.Contains("Password does not match"))
                    {
                        successMessage.text = "Password doesn't match";
                    }
                    else
                    {
                        successMessage.text = "Signed up Successfull!";

                        yield return new WaitForSeconds(messageduration);
                        successMessage.text = "";

                        SceneManager.LoadScene("Login");
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
}
