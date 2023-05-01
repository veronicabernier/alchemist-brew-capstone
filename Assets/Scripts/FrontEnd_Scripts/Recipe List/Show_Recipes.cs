using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Show_Recipes : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        string url = PostInformation.address + PostInformation.userid + "/brew list";

            UnityWebRequest ww = UnityWebRequest.Get(url);
        
        yield return ww.SendWebRequest();

        if (ww.result == UnityWebRequest.Result.Success)
        {
            string data = ww.downloadHandler.text;
            Debug.Log(data);
        }
        else {
            Debug.Log("UnityWebRequest Error:" + ww.error);
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
