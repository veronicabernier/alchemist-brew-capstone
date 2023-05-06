using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System;

public class ProfileScript : MonoBehaviour

{ 
    [Header("TextFields")]
    public TextMeshProUGUI emailText;
    public TextMeshProUGUI genderText;
    public TextMeshProUGUI locationText;
    public TextMeshProUGUI usernameText;
    public TextMeshProUGUI birthDate;

    [Header("Other")]
    public GameObject emptyText;
    public GameObject spinner;

    public static ProfileInfo profileInfo;

    void Start()
    {
        StartCoroutine(GetUserData());
    }



    IEnumerator GetUserData()
    {
        

        spinner.SetActive(true);
        UnityWebRequest request = UnityWebRequest.Get(PostInformation.address + PostInformation.userid + "/profile");
        yield return request.SendWebRequest();
        spinner.SetActive(false);

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            emptyText.GetComponent<TextMeshProUGUI>().text = request.error;
            emptyText.SetActive(true);
        }
        else
        {
            string json = request.downloadHandler.text;
            ProfileInfo profileInfo = JsonUtility.FromJson<ProfileInfoReceiver>(json).ProfileInfo;
            TimeSpan ts = new TimeSpan(10, 30, 0);
            profileInfo.birth_date = (DateTime.Parse(profileInfo.birth_date) + ts).ToString("yyyy-MM-dd");
            usernameText.text = profileInfo.username;
            emailText.text = profileInfo.email;
            genderText.text = profileInfo.gender;
            locationText.text = profileInfo.location;
            birthDate.text = profileInfo.birth_date;

            PostInformation.ProfileInfo = profileInfo;
        }
    }
}

[System.Serializable]
public class ProfileInfoReceiver
{
    public ProfileInfo ProfileInfo;
}

[System.Serializable]
public class ProfileInfo
{
    public int userid;
    public string username;
    public string email;
    public string gender;
    public string location;
    public string birth_date;
    public string password;
    public bool private_profile;
    public bool confirmation;
}




/*
{
    public TMP_Text birthDateText;
    public TMP_Text emailText;
    public TMP_Text genderText;
    public TMP_Text locationText;
    public TMP_Text privateProfileText;
    public TMP_Text userIdText;
    public TMP_Text usernameText;

    void Start()
    {
        string url = "http://127.0.0.1:5000/1/profile";
        StartCoroutine(GetProfileData(url));

        if (usernameText == null)
        {
            usernameText = GameObject.FindGameObjectWithTag("username").GetComponent<TMP_Text>();
        }
    }

    IEnumerator GetProfileData(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                // Parse the JSON response
                string jsonResponse = webRequest.downloadHandler.text;
                Debug.Log("JSON response: " + jsonResponse);
                ProfileData profileData = JsonUtility.FromJson<ProfileData>(jsonResponse);
                Debug.Log("profileData: " + profileData);

                // Display the data in TextMeshPro
                if (profileData != null && profileData.Profile.Length > 0)
                {
                    *//*birthDateText.text = "Birth Date: " + profileData.Profile[0].birth_date;
                    emailText.text = "Email: " + profileData.Profile[0].email;
                    genderText.text = "Gender: " + profileData.Profile[0].gender;
                    locationText.text = "Location: " + profileData.Profile[0].location;
                    privateProfileText.text = "Private Profile: " + profileData.Profile[0].private_profile.ToString();
                    userIdText.text = "User ID: " + profileData.Profile[0].userid.ToString();*//*
                    usernameText.text = "Username: " + profileData.Profile[0].username;
                }
                else
                {
                    Debug.LogWarning("Profile data is null or empty");
                }
            }
        }
    }
}

    [System.Serializable]
public class ProfileData
{
    public Profile[] Profile;
}

[System.Serializable]
public class Profile
{
    public string birth_date;
    public string email;
    public string gender;
    public string location;
    public bool private_profile;
    public int userid;
    public string username;
}


*/

/*
{
    // URL for the user profile API
    private string profileUrl = "http://127.0.0.1:5000/<userid>/profile";

    // UI Text object to display the user's name and email
    public TextMeshProUGUI userProfileText;

    // Function to retrieve the user profile
    IEnumerator GetProfile(string userid)
    {
        // Add the userid to the URL
        string url = profileUrl.Replace("<userid>", userid);

        // Create a UnityWebRequest to send a GET request to the API
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Send the request
            yield return webRequest.SendWebRequest();

            // Check for errors
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                // Parse the JSON data returned by the API
                string json = webRequest.downloadHandler.text;
                UserProfileData[] profileData = JSonhelper.FromJson<UserProfileData>(json);

                // Display the user's name and email in the UI Text object
                if (profileData.Length > 0)
                {
                    UserProfileData userData = profileData[0];
                    userProfileText.text = "Name: " + userData.name + "\nEmail: " + userData.email;
                }
            }
        }
    }
    void Start()
    {
        // Call the GetProfile function with a user ID
        StartCoroutine(GetProfile("2"));
    }
}




// A simple class to hold the user profile data
[System.Serializable]
public class UserProfileData
{
    public string name;
    public string email;
}
*/