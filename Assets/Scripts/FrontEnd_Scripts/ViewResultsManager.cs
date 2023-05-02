using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ViewResultsManager : MonoBehaviour
{
    [Header("General")]
    public GameObject generalScorePrefab;
    public GameObject generalSeeMoreText;
    public GameObject seeMoreParent;
    public GameObject seeMorePanel;
    public TextMeshProUGUI seeMoreTitle;

    [Header("Espresso")]
    public RectTransform espressoParent;
    public TextMeshProUGUI espressoHighScore;
    public GameObject emptyTextEspresso;
    public GameObject spinnerEspresso;

    [Header("Drip")]
    public RectTransform dripParent;
    public TextMeshProUGUI dripHighScore;
    public GameObject emptyTextDrip;
    public GameObject spinnerDrip;

    [Header("Mokapot")]
    public RectTransform mokapotParent;
    public TextMeshProUGUI mokapotHighScore;
    public GameObject emptyTextMokapot;
    public GameObject spinnerMokapot;

    [Header("Chemex")]
    public RectTransform chemexParent;
    public TextMeshProUGUI chemexHighScore;
    public GameObject emptyTextChemex;
    public GameObject spinnerChemex;

    private EspressoScoreData[] espressoScores;
    private DripScoreData[] dripScores;
    private MokapotScoreData[] mokapotScores;
    private ChemexScoreData[] chemexScores;


    void Start()
    {
        StartCoroutine(GetEspressoScores());
        StartCoroutine(GetEspressoHighest());

        StartCoroutine(GetDripScores());
        StartCoroutine(GetDripHighest());

        StartCoroutine(GetMokapotScores());
        StartCoroutine(GetMokapotHighest());

        StartCoroutine(GetChemexScores());
        StartCoroutine(GetChemexHighest());
    }

    IEnumerator GetEspressoScores()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(PostInformation.address + PostInformation.userid + "/espresso_scores"))
        {
            spinnerEspresso.SetActive(true);
            yield return webRequest.SendWebRequest();
            spinnerEspresso.SetActive(false);

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

    IEnumerator GetEspressoHighest()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(PostInformation.address + PostInformation.userid + "/espresso_highscore"))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error retrieving score data: " + webRequest.error);
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
                EspressoScoreData[] highScoreList = JsonUtility.FromJson<ScoreReceiverEspresso>(webRequest.downloadHandler.text).Attempts;

                if (highScoreList == null)
                {
                    Debug.Log("Scores array is null.");
                    espressoHighScore.text = "Espresso High Score: 0%";
                }
                else if (highScoreList.Length > 0)
                {
                    espressoHighScore.text = "Espresso High Score: " + highScoreList[0].grade + "%";
                }
            }
        }
    }

    IEnumerator GetDripScores()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(PostInformation.address + PostInformation.userid + "/drip_scores"))
        {
            spinnerDrip.SetActive(true);
            yield return webRequest.SendWebRequest();
            spinnerDrip.SetActive(false);

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error retrieving score data: " + webRequest.error);
                emptyTextDrip.GetComponent<TextMeshProUGUI>().text = webRequest.error;
                emptyTextDrip.SetActive(true);
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
                dripScores = JsonUtility.FromJson<ScoreReceiverDrip>(webRequest.downloadHandler.text).Attempts;

                if (dripScores == null)
                {
                    Debug.Log("Scores array is null.");
                }
                else if (dripScores.Length == 0)
                {
                    Debug.Log("Scores array is empty.");
                    emptyTextDrip.SetActive(true);
                }

                foreach (DripScoreData score in dripScores)
                {
                    GameObject item = Instantiate(generalScorePrefab, dripParent);
                    item.GetComponentInChildren<TextMeshProUGUI>().text = DateTime.Parse(score.dateObtained).ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss") + " : " + score.grade.ToString() + " %";
                    item.GetComponentInChildren<Button>().onClick.AddListener(delegate { dripOpenSeeMore(score); });
                }
                GridLayoutGroup glg = dripParent.GetComponent<GridLayoutGroup>();
                float newHeight = (glg.cellSize.y + glg.padding.top + glg.padding.bottom + glg.spacing.y) * dripScores.Length;
                dripParent.sizeDelta = new Vector2(dripParent.sizeDelta.x, newHeight);
                dripParent.anchoredPosition = new Vector2(dripParent.anchoredPosition.x, 0);

            }

        }
    }

    IEnumerator GetDripHighest()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(PostInformation.address + PostInformation.userid + "/drip_highscore"))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error retrieving score data: " + webRequest.error);
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
                DripScoreData[] highScoreList = JsonUtility.FromJson<ScoreReceiverDrip>(webRequest.downloadHandler.text).Attempts;

                if (highScoreList == null)
                {
                    Debug.Log("Scores array is null.");
                    dripHighScore.text = "Drip High Score: 0%";
                }
                else if (highScoreList.Length > 0)
                {
                    dripHighScore.text = "Drip High Score: " + highScoreList[0].grade + "%";
                }
            }
        }
    }

    IEnumerator GetMokapotScores()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(PostInformation.address + PostInformation.userid + "/mokapot_scores"))
        {
            spinnerMokapot.SetActive(true);
            yield return webRequest.SendWebRequest();
            spinnerMokapot.SetActive(false);

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error retrieving score data: " + webRequest.error);
                emptyTextMokapot.GetComponent<TextMeshProUGUI>().text = webRequest.error;
                emptyTextMokapot.SetActive(true);
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
                mokapotScores = JsonUtility.FromJson<ScoreReceiverMokapot>(webRequest.downloadHandler.text).Attempts;

                if (mokapotScores == null)
                {
                    Debug.Log("Scores array is null.");
                }
                else if (mokapotScores.Length == 0)
                {
                    Debug.Log("Scores array is empty.");
                    emptyTextMokapot.SetActive(true);
                }

                foreach (MokapotScoreData score in mokapotScores)
                {
                    GameObject item = Instantiate(generalScorePrefab, mokapotParent);
                    item.GetComponentInChildren<TextMeshProUGUI>().text = DateTime.Parse(score.dateObtained).ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss") + " : " + score.grade.ToString() + " %";
                    item.GetComponentInChildren<Button>().onClick.AddListener(delegate { mokapotOpenSeeMore(score); });
                }
                GridLayoutGroup glg = mokapotParent.GetComponent<GridLayoutGroup>();
                float newHeight = (glg.cellSize.y + glg.padding.top + glg.padding.bottom + glg.spacing.y) * mokapotScores.Length;
                mokapotParent.sizeDelta = new Vector2(mokapotParent.sizeDelta.x, newHeight);
                mokapotParent.anchoredPosition = new Vector2(mokapotParent.anchoredPosition.x, 0);

            }

        }
    }

    IEnumerator GetMokapotHighest()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(PostInformation.address + PostInformation.userid + "/mokapot_highscore"))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error retrieving score data: " + webRequest.error);
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
                MokapotScoreData[] highScoreList = JsonUtility.FromJson<ScoreReceiverMokapot>(webRequest.downloadHandler.text).Attempts;

                if (highScoreList == null)
                {
                    Debug.Log("Scores array is null.");
                    mokapotHighScore.text = "Mokapot High Score: 0%";
                }
                else if (highScoreList.Length > 0)
                {
                    mokapotHighScore.text = "Mokapot High Score: " + highScoreList[0].grade + "%";
                }
            }
        }
    }

    IEnumerator GetChemexScores()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(PostInformation.address + PostInformation.userid + "/chemex_scores"))
        {
            spinnerChemex.SetActive(true);
            yield return webRequest.SendWebRequest();
            spinnerChemex.SetActive(false);

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error retrieving score data: " + webRequest.error);
                emptyTextChemex.GetComponent<TextMeshProUGUI>().text = webRequest.error;
                emptyTextChemex.SetActive(true);
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
                chemexScores = JsonUtility.FromJson<ScoreReceiverChemex>(webRequest.downloadHandler.text).Attempts;

                if (chemexScores == null)
                {
                    Debug.Log("Scores array is null.");
                }
                else if (chemexScores.Length == 0)
                {
                    Debug.Log("Scores array is empty.");
                    emptyTextChemex.SetActive(true);
                }

                foreach (ChemexScoreData score in chemexScores)
                {
                    GameObject item = Instantiate(generalScorePrefab, chemexParent);
                    item.GetComponentInChildren<TextMeshProUGUI>().text = DateTime.Parse(score.dateObtained).ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss") + " : " + score.grade.ToString() + " %";
                    item.GetComponentInChildren<Button>().onClick.AddListener(delegate { chemexOpenSeeMore(score); });
                }
                GridLayoutGroup glg = chemexParent.GetComponent<GridLayoutGroup>();
                float newHeight = (glg.cellSize.y + glg.padding.top + glg.padding.bottom + glg.spacing.y) * chemexScores.Length;
                chemexParent.sizeDelta = new Vector2(chemexParent.sizeDelta.x, newHeight);
                chemexParent.anchoredPosition = new Vector2(chemexParent.anchoredPosition.x, 0);

            }

        }
    }

    IEnumerator GetChemexHighest()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(PostInformation.address + PostInformation.userid + "/chemex_highscore"))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error retrieving score data: " + webRequest.error);
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
                ChemexScoreData[] highScoreList = JsonUtility.FromJson<ScoreReceiverChemex>(webRequest.downloadHandler.text).Attempts;

                if (highScoreList == null)
                {
                    Debug.Log("Scores array is null.");
                    chemexHighScore.text = "Chemex High Score: 0%";
                }
                else if (highScoreList.Length > 0)
                {
                    chemexHighScore.text = "Chemex High Score: " + highScoreList[0].grade + "%";
                }
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

    public void dripOpenSeeMore(DripScoreData data)
    {
        foreach (Transform child in seeMoreParent.transform)
        {
            Destroy(child.gameObject);
        }

        seeMoreTitle.text = "Drip Score: " + data.scoreTotal + "/" + data.evalTotal;

        //weightScoreTotal;
        Instantiate(generalSeeMoreText, seeMoreParent.transform).GetComponent<TextMeshProUGUI>().text = "Weight Score: " + data.weightScore + "/" + data.weightScoreTotal;
        //reservoirScore;
        Instantiate(generalSeeMoreText, seeMoreParent.transform).GetComponent<TextMeshProUGUI>().text = "Reservoir Score: " + data.reservoirScore + "/" + data.reservoirScoreTotal;
        //grindScore;
        Instantiate(generalSeeMoreText, seeMoreParent.transform).GetComponent<TextMeshProUGUI>().text = "Grind Score: " + data.grindScore + "/" + data.grindScoreTotal;
        //chooseFilterScore;
        Instantiate(generalSeeMoreText, seeMoreParent.transform).GetComponent<TextMeshProUGUI>().text = "Choose Filter Score: " + data.chooseFilterScore + "/" + data.chooseFilterScoreTotal;
        //refillReservoirScore;
        Instantiate(generalSeeMoreText, seeMoreParent.transform).GetComponent<TextMeshProUGUI>().text = "Refill Reservoir Score: " + data.refillReservoirScore + "/" + data.refillReservoirScoreTotal;
        //brewScore;
        Instantiate(generalSeeMoreText, seeMoreParent.transform).GetComponent<TextMeshProUGUI>().text = "Brew Score: " + data.brewScore + "/" + data.brewScoreTotal;
        //serveScore;
        Instantiate(generalSeeMoreText, seeMoreParent.transform).GetComponent<TextMeshProUGUI>().text = "Serve Score: " + data.serveScore + "/" + data.serveScoreTotal;

        seeMorePanel.SetActive(true);
    }

    public void mokapotOpenSeeMore(MokapotScoreData data)
    {
        foreach (Transform child in seeMoreParent.transform)
        {
            Destroy(child.gameObject);
        }

        seeMoreTitle.text = "Mokapot Score: " + data.scoreTotal + "/" + data.evalTotal;

        //weightScoreTotal;
        Instantiate(generalSeeMoreText, seeMoreParent.transform).GetComponent<TextMeshProUGUI>().text = "Weight Score: " + data.weightScore + "/" + data.weightScoreTotal;
        //grindScore;
        Instantiate(generalSeeMoreText, seeMoreParent.transform).GetComponent<TextMeshProUGUI>().text = "Grind Score: " + data.grindScore + "/" + data.grindScoreTotal;
        //chooseWaterScore;
        Instantiate(generalSeeMoreText, seeMoreParent.transform).GetComponent<TextMeshProUGUI>().text = "Choose Water Score: " + data.chooseWaterScore + "/" + data.chooseWaterScoreTotal;
        //addCoffeeScore;
        Instantiate(generalSeeMoreText, seeMoreParent.transform).GetComponent<TextMeshProUGUI>().text = "Add Coffee Score: " + data.addCoffeeScore + "/" + data.addCoffeeScoreTotal;
        //putTogetherScore;
        Instantiate(generalSeeMoreText, seeMoreParent.transform).GetComponent<TextMeshProUGUI>().text = "Put Together Score: " + data.putTogetherScore + "/" + data.putTogetherScoreTotal;
        //stoveScore;
        Instantiate(generalSeeMoreText, seeMoreParent.transform).GetComponent<TextMeshProUGUI>().text = "Stove Score: " + data.stoveScore + "/" + data.stoveScoreTotal;
        //serveScore;
        Instantiate(generalSeeMoreText, seeMoreParent.transform).GetComponent<TextMeshProUGUI>().text = "Serve Score: " + data.serveScore + "/" + data.serveScoreTotal;

        seeMorePanel.SetActive(true);
    }

    public void chemexOpenSeeMore(ChemexScoreData data)
    {
        foreach (Transform child in seeMoreParent.transform)
        {
            Destroy(child.gameObject);
        }

        seeMoreTitle.text = "Chemex Score: " + data.scoreTotal + "/" + data.evalTotal;

        //weightScoreTotal;
        Instantiate(generalSeeMoreText, seeMoreParent.transform).GetComponent<TextMeshProUGUI>().text = "Weight Score: " + data.weightScore + "/" + data.weightScoreTotal;
        //grindScore;
        Instantiate(generalSeeMoreText, seeMoreParent.transform).GetComponent<TextMeshProUGUI>().text = "Grind Score: " + data.grindScore + "/" + data.grindScoreTotal;
        //wetGroundsScore;
        Instantiate(generalSeeMoreText, seeMoreParent.transform).GetComponent<TextMeshProUGUI>().text = "Wet Grounds Score: " + data.wetGroundsScore + "/" + data.weightScoreTotal;
        //addWaterScore;
        Instantiate(generalSeeMoreText, seeMoreParent.transform).GetComponent<TextMeshProUGUI>().text = "Add Water Score: " + data.addWaterScore + "/" + data.addWaterScoreTotal;
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
public class ScoreReceiverDrip
{
    public DripScoreData[] Attempts;
}

[System.Serializable]
public class ScoreReceiverMokapot
{
    public MokapotScoreData[] Attempts;
}

[System.Serializable]
public class ScoreReceiverChemex
{
    public ChemexScoreData[] Attempts;
}

[System.Serializable]
public class EspressoScoreData : EspressoScore 
{
    public int espresso_scoreid;
    public int evalTotal;
    public int scoreTotal;
    public float grade;
    public string dateObtained;
}


[System.Serializable]
public class DripScoreData : DripScore
{
    public int drip_scoreid;
    public int evalTotal;
    public int scoreTotal;
    public float grade;
    public string dateObtained;
}

[System.Serializable]
public class MokapotScoreData : MokapotScore
{
    public int mokapot_scoreid;
    public int evalTotal;
    public int scoreTotal;
    public float grade;
    public string dateObtained;
}

[System.Serializable]
public class ChemexScoreData : ChemexScore
{
    public int chemex_scoreid;
    public int evalTotal;
    public int scoreTotal;
    public float grade;
    public string dateObtained;
}