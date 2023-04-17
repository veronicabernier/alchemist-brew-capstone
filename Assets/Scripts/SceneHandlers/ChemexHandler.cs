using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChemexHandler : MonoBehaviour
{
    [System.Serializable]
    public class Level
    {
        public LevelTypes name;

        public GameObject prefab;
        public GameObject button;

        [System.NonSerialized]
        public GameObject instance;
        [System.NonSerialized]
        public int curScore;
        [System.NonSerialized]
        public int curScoreTotal;
        [System.NonSerialized]
        public List<string> comments;
    }

    public enum LevelTypes
    {
        Weighing,
        Grind,
        WetGrounds,
        AddWater,
        Serve
    }

    [LabeledArrayAttribute(typeof(LevelTypes))]
    public Level[] levels = new Level[System.Enum.GetValues(typeof(LevelTypes)).Length];
    public GameObject levelsMenu;
    public TextMeshProUGUI feedbackName;
    public TextMeshProUGUI feedback;
    public TextMeshProUGUI menuLabel;

    public GameObject finalResultsMenu;
    public GameObject finalScorePefab;
    public float spaceBetweenScores = 0.5f;

    private ChemexScore chemexScore = new ChemexScore();
    private int curLevel = -1;


    // Start is called before the first frame update
    void Start()
    {
        //MoveLevel();
        foreach(Level l in levels)
        {
            l.button.GetComponent<Button>().enabled = false;
            l.button.GetComponent<Image>().color = new Color(0.35f, 0.35f, 0.35f, 0.8f);
        }
        UpdateLevelMenu(null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //to be called by level button
    public void MoveLevel()
    {
        levelsMenu.SetActive(false);
        levels[curLevel].instance = Instantiate(levels[curLevel].prefab, transform);
        if(levels[curLevel].name == LevelTypes.Grind)
        {
            GrindController gc = levels[curLevel].instance.GetComponent<GrindController>();
            gc.wantedGrindSetting = SnapToCloserX.SettingType.Medium;
        }
    }


    public void UpdateLevelMenu(SingleScore myScore)
    {
        if (curLevel > -1)
        {
            levels[curLevel].button.GetComponent<Button>().enabled = false;
            levels[curLevel].button.GetComponent<Image>().color = new Color(0.35f, 0.35f, 0.35f, 0.8f);

            //update feedback
            feedbackName.text = levels[curLevel].name.ToString() + " Result:" + myScore.curScore.ToString() + "/" + myScore.curScoreTotal.ToString();
            String feedbackText = "";
            if(myScore.comments.Count == 0)
            {
                feedbackText += "Perfect!";
            }
            else
            {
                foreach(string c in myScore.comments)
                {
                    if(feedbackText != "")
                    {
                        feedbackText += "\n";
                    }
                    feedbackText += c;
                }
            }
            feedback.text = feedbackText;
            if(curLevel < levels.Length - 1)
            {
                menuLabel.text = "Next Level";
            }
            else
            {
                menuLabel.text = "Done!";
            }
        }
        curLevel += 1;
        levels[curLevel].button.GetComponent<Button>().enabled = true;
        levels[curLevel].button.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

    }

    public void StopLevel(SingleScore myScore)
    {
        //save score 
        SaveScores(myScore.curScore, myScore.curScoreTotal, myScore.comments);
        Debug.Log(JsonUtility.ToJson(chemexScore));

        //destroy level
        Destroy(levels[curLevel].instance);

        //move level
        if(curLevel < levels.Length - 1)
        {
            levelsMenu.SetActive(true);
            UpdateLevelMenu(myScore);
        }
        else
        {
            ShowFinalResults();
        }
    }

    private void ShowFinalResults()
    {
        finalResultsMenu.SetActive(true);
        for(int i = 0; i < levels.Length; i++)
        {
            GameObject newText = Instantiate(finalScorePefab, finalResultsMenu.transform);
            newText.transform.localPosition = new Vector3(newText.transform.localPosition.x, newText.transform.localPosition.y - (i*spaceBetweenScores), newText.transform.localPosition.z);
            newText.GetComponent<TextMeshProUGUI>().text = (i+1).ToString() + ". " + levels[i].name.ToString() + ":    "+ levels[i].curScore.ToString() + "/" + levels[i].curScoreTotal.ToString();
        }
        SendToDatabase();
    }

    void SaveScores(int curScore, int curTotalScore, List<string> comments)
    {
        levels[curLevel].curScore = curScore;
        levels[curLevel].curScoreTotal = curTotalScore;
        levels[curLevel].comments = comments;

        if (levels[curLevel].name == LevelTypes.Weighing)
        {
            chemexScore.weightScore = curScore;
            chemexScore.weightScoreTotal = curTotalScore;
        }
        else if (levels[curLevel].name == LevelTypes.Grind)
        {
            chemexScore.grindScore = curScore;
            chemexScore.grindScoreTotal = curTotalScore;
        }
        else if (levels[curLevel].name == LevelTypes.WetGrounds)
        {
            chemexScore.wetGroundsScore = curScore;
            chemexScore.wetGroundsScoreTotal = curTotalScore;
        }
        else if (levels[curLevel].name == LevelTypes.AddWater)
        {
            chemexScore.addWaterScore = curScore;
            chemexScore.addWaterScoreTotal = curTotalScore;
        }
        else if (levels[curLevel].name == LevelTypes.Serve)
        {
            chemexScore.serveScore = curScore;
            chemexScore.serveScoreTotal = curTotalScore;
        }
    }

    void SendToDatabase()
    {
        int userId = PostInformation.userid;
        WWWForm form = new WWWForm();
        form.AddField("weightScore", chemexScore.weightScore);
        form.AddField("weightScoreTotal", chemexScore.weightScoreTotal);
        form.AddField("grindScore", chemexScore.grindScore);
        form.AddField("grindScoreTotal", chemexScore.grindScoreTotal);
        form.AddField("wetGroundsScore", chemexScore.wetGroundsScore);
        form.AddField("wetGroundsScoreTotal", chemexScore.wetGroundsScoreTotal);
        form.AddField("addWaterScore", chemexScore.addWaterScore);
        form.AddField("addWaterScoreTotal", chemexScore.addWaterScoreTotal);
        form.AddField("serveScore", chemexScore.serveScore);
        form.AddField("serveScoreTotal", chemexScore.serveScoreTotal);

        StartCoroutine(PostInformation.PostRequest(PostInformation.address + userId.ToString() + "/chemex_simulation", form));
    }
}
