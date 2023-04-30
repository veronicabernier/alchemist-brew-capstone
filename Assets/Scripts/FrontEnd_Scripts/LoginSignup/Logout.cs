using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Logout : MonoBehaviour
{

    public string afterSceneName;

    public void OnLogout()
    {
        PostInformation.userid = -1;
        new SceneChanger().changeScene(afterSceneName);
    }
}
