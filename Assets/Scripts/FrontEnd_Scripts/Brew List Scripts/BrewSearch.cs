using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.Linq;
public class BrewSearch : MonoBehaviour
{
    public GameObject BrewItemPrefab;
    public RectTransform contentTransform;
    public GameObject emptyText;
    private BrewData[] brews;

    public Dropdown tags;


    void Start()
    {
        tags.onValueChanged.AddListener(delegate { onTagchange(); });
;
        StartCoroutine(GetBrews(PostInformation.address + PostInformation.userid + "/search_all_brews"));
    }

    public void onTagchange()
    {
        if(tags.value == 0)
        {
            //return all
            StartCoroutine(GetBrews(PostInformation.address + PostInformation.userid + "/search_all_brews"));
        }
        else
        {
            StartCoroutine(GetBrews(PostInformation.address + PostInformation.userid + "/search/" + tags.value));
        }
    }

    IEnumerator GetBrews(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error retrieving recipe data: " + webRequest.error);
                emptyText.GetComponent<TextMeshProUGUI>().text = webRequest.error;
                emptyText.SetActive(true);
            }
            else
            {
                emptyText.SetActive(false);
                foreach (Transform child in contentTransform.transform)
                {
                    Destroy(child.gameObject);
                }

                string brewData = webRequest.downloadHandler.text;
                brews = JSonHelperBrew.FromJson<BrewData>(brewData);
                if (brews == null)
                {
                    emptyText.SetActive(true);
                }
                else if (brews.Length == 0)
                {
                    emptyText.SetActive(true);
                }

                foreach (BrewData i in brews)
                {
                    GameObject brewItem = Instantiate(BrewItemPrefab, contentTransform);
                    brewItem.GetComponent<BrewSearchItem>().SetBrew(i);
                }
                GridLayoutGroup glg = contentTransform.GetComponent<GridLayoutGroup>();
                float newHeight = (glg.cellSize.y + glg.padding.top + glg.padding.bottom + glg.spacing.y) * brews.Length;
                contentTransform.sizeDelta = new Vector2(contentTransform.sizeDelta.x, newHeight);
                contentTransform.anchoredPosition = new Vector2(contentTransform.anchoredPosition.x, 0);

            }

        }
    }
}

