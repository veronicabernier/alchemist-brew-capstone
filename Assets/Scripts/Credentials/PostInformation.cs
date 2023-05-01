
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PostInformation : MonoBehaviour
{
    public static int userid = 16;
    public static string address = "http://127.0.0.1:5000/";

    public static ProfileInfo ProfileInfo;

    public static IEnumerator PostRequest(string uri, WWWForm postData)
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
