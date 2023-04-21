using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class Dropdown_logbook : MonoBehaviour
{
    public Dropdown dropdown;

    IEnumerator Start()
    {
        // Replace the URL with your Flask route URL
        using (UnityWebRequest www = UnityWebRequest.Get("http://127.0.0.1:5000/basic_tags"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Parse the JSON data and get the "inner_section" values
                string jsonData = www.downloadHandler.text;
                RootObject root = JsonUtility.FromJson<RootObject>(jsonData);

                List<string> options = new List<string>();
                foreach (var attempt in root.Attempts)
                {
                    string innerSection = attempt.inner_section;
                    if (!string.IsNullOrEmpty(innerSection) && !options.Contains(innerSection))
                    {
                        options.Add(innerSection);
                    }
                }

                // Add the "inner_section" values to the dropdown menu
                dropdown.AddOptions(options);
            }
        }
    }

    // The RootObject class is used to deserialize the JSON data
    [System.Serializable]
    public class RootObject
    {
        public List<Attempt> Attempts;
    }

    [System.Serializable]
    public class Attempt
    {
        public string inner_section;
        public string middle_section;
        public string outer_section;
        public int tagid;
    }
}




/*{
    public Dropdown dropdown;

    void Start()
    {
        *//* dropdown = GetComponent<Dropdown>();
         Debug.Log("Dropdown reference: " + dropdown);*//*
        dropdown = GameObject.FindGameObjectWithTag("tags").GetComponent<Dropdown>();
        StartCoroutine(GetOptions());
        Debug.Log("Dropdown reference: " + dropdown);
    }
    

    IEnumerator GetOptions()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://127.0.0.1:5000/basic_tags"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string dataJson = www.downloadHandler.text;
                Data[] data = JSonhelper.FromJson<Data>(dataJson);

                List<string> options = new List<string>();

                for (int i = 0; i < data.Length; i++)
                {
                    options.Add(data[i].inner_section);
                }
                Debug.Log("Dropdown options: " + string.Join(", ", options));

                if (dropdown != null)
                {
                    dropdown.ClearOptions();
                    dropdown.AddOptions(options);
                    Debug.Log("Dropdown options count: " + dropdown.options.Count);
                }
                else
                {
                    Debug.Log("Dropdown reference is null.");
                }
            }
            else
            {
                Debug.Log("Error retrieving data: " + www.error);
            }
        }
    }
}

[System.Serializable]
public class Data
{
    public string inner_section;
}
*/




/*{
    public Dropdown dropdown;

    void Start()
    {
        StartCoroutine(GetOptions());
    }

    IEnumerator GetOptions()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://127.0.0.1:5000/basic_tags"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string dataJson = www.downloadHandler.text;
                Data[] data = JsonHelper.FromJson<Data>(dataJson);

                string[] options = new string[data.Length];

                for (int i = 0; i < data.Length; i++)
                {
                    options[i] = data[i].inner_section;
                }

                dropdown.ClearOptions();
                dropdown.AddOptions(options);
            }
            else
            {
                Debug.Log("Error retrieving data: " + www.error);
            }
        }
    }
}

[System.Serializable]
public class Data
{
    public string inner_section;
}
*/

