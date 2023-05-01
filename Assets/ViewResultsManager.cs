using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ViewResultsManager : MonoBehaviour
{
    public GameObject generalScorePrefab;
    public GameObject generalSeeMoreText;

    public RectTransform espressoParent;
    
    public GameObject emptyTextEspresso;
    public GameObject seeMoreParent;
    public GameObject seeMorePanel;
    public TextMeshProUGUI seeMoreTitle;

    private EspressoScoreData[] espressoScores;


    void Start()
    {
        StartCoroutine(GetEspressoScores());

    }

    IEnumerator GetEspressoScores()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(PostInformation.address + PostInformation.userid + "/espresso_scores"))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error retrieving score data: " + webRequest.error);
                emptyTextEspresso.GetComponent<TextMeshProUGUI>().text = webRequest.error;
                emptyTextEspresso.SetActive(true);
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
                espressoScores = JsonUtility.FromJson<ScoreReceiverEspresso>(webRequest.downloadHandler.text).Attempts;
                
                if (espressoScores == null)
                {
                    Debug.Log("Scores array is null.");
                }
                else if (espressoScores.Length == 0)
                {
                    Debug.Log("Scores array is empty.");
                    emptyTextEspresso.SetActive(true);
                }

                foreach (EspressoScoreData score in espressoScores)
                {
                    GameObject espressoItem = Instantiate(generalScorePrefab, espressoParent);
                    espressoItem.GetComponentInChildren<TextMeshProUGUI>().text = DateTime.Parse(score.dateObtained).ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss") + " : " + score.grade.ToString() + " %";
                    espressoItem.GetComponentInChildren<Button>().onClick.AddListener(delegate { espressoOpenSeeMore(score); });
                }
                GridLayoutGroup glgEspresso = espressoParent.GetComponent<GridLayoutGroup>();
                float newHeight = (glgEspresso.cellSize.y + glgEspresso.padding.top + glgEspresso.padding.bottom + glgEspresso.spacing.y) * espressoScores.Length;
                espressoParent.sizeDelta = new Vector2(espressoParent.sizeDelta.x, newHeight);
                espressoParent.anchoredPosition = new Vector2(espressoParent.anchoredPosition.x, 0);

            }

        }
    }

    public void espressoOpenSeeMore(EspressoScoreData data)
    {
        foreach(Transform child in seeMoreParent.transform){
            Destroy(child.gameObject);
        }

        seeMoreTitle.text = "Espresso Score: " + data.scoreTotal + "/" + data.evalTotal;

        //weightScoreTotal;
        Instantiate(generalSeeMoreText, seeMoreParent.transform).GetComponent<TextMeshProUGUI>().text = "Weight Score: " + data.weightScore + "/" + data.weightScoreTotal;
        //reservoirScore;
        Instantiate(generalSeeMoreText, seeMoreParent.transform).GetComponent<TextMeshProUGUI>().text = "Reservoir Score: " + data.reservoirScore + "/" + data.reservoirScoreTotal;
        //powerOnScore;
        Instantiate(generalSeeMoreText, seeMoreParent.transform).GetComponent<TextMeshProUGUI>().text = "Power On Score: " + data.powerOnScore + "/" + data.powerOnScoreTotal;
        //grindScore;
        Instantiate(generalSeeMoreText, seeMoreParent.transform).GetComponent<TextMeshProUGUI>().text = "Grind Score: " + data.grindScore + "/" + data.grindScoreTotal;
        //tampScore;
        Instantiate(generalSeeMoreText, seeMoreParent.transform).GetComponent<TextMeshProUGUI>().text = "Tamp Score: " + data.tampScore + "/" + data.tampScoreTotal;
        //brewScore;
        Instantiate(generalSeeMoreText, seeMoreParent.transform).GetComponent<TextMeshProUGUI>().text = "Brew Score: " + data.brewScore + "/" + data.brewScoreTotal;
        //serveScore;
        Instantiate(generalSeeMoreText, seeMoreParent.transform).GetComponent<TextMeshProUGUI>().text = "Serve Score: " + data.serveScore + "/" + data.serveScoreTotal;

        seeMorePanel.SetActive(true);
    }

    public void closeSeeMore()
    {
        foreach (Transform child in seeMoreParent.transform)
        {
            Destroy(child.gameObject);
        }
        seeMorePanel.SetActive(false);

    }
}

[System.Serializable]
public class ScoreReceiverEspresso
{
    public EspressoScoreData[] Attempts;
}

[System.Serializable]
public class EspressoScoreData : EspressoScore 
{
    public int espreso_scoreid;
    public int evalTotal;
    public int scoreTotal;
    public float grade;
    public string dateObtained;
}
