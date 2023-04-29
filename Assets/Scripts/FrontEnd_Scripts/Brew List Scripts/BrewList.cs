using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.Linq;
public class BrewList : MonoBehaviour
{
    public GameObject BrewItemPrefab;
    public RectTransform contentTransform;
    private BrewData[] brews;

    private List<string> brands = new List<string>() { "All" };
    private List<string> roast = new List<string>() { "All" };
    private List<string> tag = new List<string>() { "All" };

    void Start()
    {
        StartCoroutine(GetBrews());
    }

    IEnumerator GetBrews()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("http://127.0.0.1:5000/1/brew%20list"))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error retrieving recipe data: " + webRequest.error);
            }
            else
            {
                string brewData = webRequest.downloadHandler.text;
                brews = JSonHelperBrew.FromJson<BrewData>(brewData);
                if (brews == null)
                {
                    Debug.Log("Recipes array is null.");
                }
                else if (brews.Length == 0)
                {
                    Debug.Log("Recipes array is empty.");
                }
                Debug.Log("Roast: " + brews[0].bean_type);

                foreach (BrewData i in brews)
                {
                    GameObject brewItem = Instantiate(BrewItemPrefab, contentTransform);
                    brewItem.GetComponent<BrewItem>().SetBrew(i);
                }
                GameObject firstRecipeItem = contentTransform.GetChild(0).gameObject;
                Destroy(firstRecipeItem);
                float newHeight = BrewItemPrefab.GetComponent<RectTransform>().sizeDelta.y * brews.Length;
                contentTransform.sizeDelta = new Vector2(contentTransform.sizeDelta.x, newHeight);
                contentTransform.anchoredPosition = new Vector2(contentTransform.anchoredPosition.x, 0);
                Debug.Log("Successfully parsed " + brews.Length + " recipes.");
                Debug.Log(brews[1].brand + "recipe brand");

            }
            for (int i = 0; i < brews.Length; i++)
            {
                if (!brands.Contains(brews[i].brand))
                {
                    brands.Add(brews[i].brand);
                }
            }
            for (int i = 0; i < brews.Length; i++)
            {
                if (!roast.Contains(brews[i].roast))
                {
                    roast.Add(brews[i].roast);
                }
            }
            for (int i = 0; i < brews.Length; i++)
            {
                if (!tag.Contains(brews[i].inner_section))
                {
                    tag.Add(brews[i].inner_section);
                }
            }
            roast.Sort();
            brands.Sort();
            tag.Sort();
            /*tags.Sort();*/

        }
    }
}


[System.Serializable]
public class BrewData
{
    public string bean_type;
    public string brand;
    public string brew_method;
    public bool brew_visibility;
    public int brewid;
    public float coffee_weight;
    public string date;
    public int ext_time;
    public int ext_weight;
    public int grind_setting;
    public string inner_section;
    public string middle_section;
    public string notes;
    public string outer_section;
    public int recipeid;
    public string roast;
    public int tagid;
    public int userid;
}
