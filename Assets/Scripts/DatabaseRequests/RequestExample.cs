using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RequestExample : MonoBehaviour
{

    void Start()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", "Rey");
        form.AddField("email", "reinaldo2@upr.edu");
        form.AddField("password", "888");
        form.AddField("confirm", "888"); // confirm password
        form.AddField("private_profile", "True"); // no poner en front end
        form.AddField("birth_date", "1998-02-03");
        form.AddField("gender", "Male");
        form.AddField("location", "Puerto Rico"); // verificar si se puede coger location 

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
