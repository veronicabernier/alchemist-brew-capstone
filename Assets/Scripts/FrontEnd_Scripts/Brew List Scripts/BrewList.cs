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
    public GameObject emptyText;
    public GameObject spinner;

    private BrewData[] brews;


    void Start()
    {
        StartCoroutine(GetBrews());
    }

    IEnumerator GetBrews()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(PostInformation.address + PostInformation.userid + "/brew%20list"))
        {
            spinner.SetActive(true);
            yield return webRequest.SendWebRequest();
            spinner.SetActive(false);

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error retrieving recipe data: " + webRequest.error);
                emptyText.GetComponent<TextMeshProUGUI>().text = webRequest.error;
                emptyText.SetActive(true);
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
                    emptyText.SetActive(true);
                }

                foreach (BrewData i in brews)
                {
                    GameObject brewItem = Instantiate(BrewItemPrefab, contentTransform);
                    brewItem.GetComponent<BrewItem>().SetBrew(i);
                }
                GridLayoutGroup glg = contentTransform.GetComponent<GridLayoutGroup>();
                float newHeight = (glg.cellSize.y + glg.padding.top + glg.padding.bottom + glg.spacing.y) * brews.Length;
                contentTransform.sizeDelta = new Vector2(contentTransform.sizeDelta.x, newHeight);
                contentTransform.anchoredPosition = new Vector2(contentTransform.anchoredPosition.x, 0);

            }

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
