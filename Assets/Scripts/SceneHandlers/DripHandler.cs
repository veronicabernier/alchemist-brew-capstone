using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DripHandler : MonoBehaviour
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
        Reservoir,
        Grind,
        ChooseFilter,
        RefillReservoir,
        Brew,
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

    private DripScore dripScore = new DripScore();
    private int curLevel = -1;


    // Start is called before the first frame update
    void Start()
    {
        //MoveLevel();
        foreach(Level l in levels)
        {
            l.button.GetComponent<Button>().enabled = false;
            l.button.GetComponent<Image>().color = new Color(0.55f, 0.55f, 0.55f, 0.8f);
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
            levels[curLevel].button.GetComponent<Image>().color = new Color(0.55f, 0.55f, 0.55f, 0.8f);

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
        Debug.Log(JsonUtility.ToJson(dripScore));

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
    }

    void SaveScores(int curScore, int curTotalScore, List<string> comments)
    {
        levels[curLevel].curScore = curScore;
        levels[curLevel].curScoreTotal = curTotalScore;
        levels[curLevel].comments = comments;

        if (levels[curLevel].name == LevelTypes.Weighing)
        {
            dripScore.weightScore = curScore;
            dripScore.weightScoreTotal = curTotalScore;
        }
        else if (levels[curLevel].name == LevelTypes.Reservoir)
        {
            dripScore.reservoirScore = curScore;
            dripScore.reservoirScoreTotal = curTotalScore;
        }
        else if (levels[curLevel].name == LevelTypes.Grind)
        {
            dripScore.grindScore = curScore;
            dripScore.grindScoreTotal = curTotalScore;
        }
        else if (levels[curLevel].name == LevelTypes.ChooseFilter)
        {
            dripScore.chooseFilterScore = curScore;
            dripScore.chooseFilterScoreTotal = curTotalScore;
        }
        else if (levels[curLevel].name == LevelTypes.RefillReservoir)
        {
                dripScore.refillReservoirScore = curScore;
                dripScore.refillReservoirScoreTotal = curTotalScore;
        }
        else if (levels[curLevel].name == LevelTypes.Brew)
        {
            dripScore.brewScore = curScore;
            dripScore.brewScoreTotal = curTotalScore;
        }
        else if (levels[curLevel].name == LevelTypes.Serve)
        {
            dripScore.serveScore = curScore;
            dripScore.serveScoreTotal = curTotalScore;
        }
    }
}