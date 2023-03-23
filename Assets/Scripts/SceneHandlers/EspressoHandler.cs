using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EspressoHandler : MonoBehaviour
{
    [System.Serializable]
    public class Level
    {
        public LevelTypes name;

        public GameObject prefab;
        public GameObject button;

        [System.NonSerialized]
        public GameObject instance;
    }

    public enum LevelTypes
    {
        Weighing,
        Reservoir,
        PowerOn,
        Grind,
        Tamp,
        Brew,
        Serve
    }

    [LabeledArrayAttribute(typeof(LevelTypes))]
    public Level[] levels = new Level[System.Enum.GetValues(typeof(LevelTypes)).Length];
    public GameObject espressoMenu;

    private EspressoScore espressoScore = new EspressoScore();
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
        UpdateLevelMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //to be called by level button
    public void MoveLevel()
    {
        espressoMenu.SetActive(false);
        levels[curLevel].instance = Instantiate(levels[curLevel].prefab, transform);
    }


    public void UpdateLevelMenu()
    {
        if (curLevel > -1)
        {
            levels[curLevel].button.GetComponent<Button>().enabled = false;
            levels[curLevel].button.GetComponent<Image>().color = new Color(0.55f, 0.55f, 0.55f, 0.8f);
        }
        curLevel += 1;
        levels[curLevel].button.GetComponent<Button>().enabled = true;
        levels[curLevel].button.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

    }

    public void StopLevel(SingleScore myScore)
    {
        //save score 
        SaveScores(myScore.curScore, myScore.curScoreTotal);
        Debug.Log(JsonUtility.ToJson(espressoScore));

        //destroy level
        Destroy(levels[curLevel].instance);

        //move level
        if(curLevel < levels.Length - 1)
        {
            espressoMenu.SetActive(true);
            UpdateLevelMenu();
        }
    }

    void SaveScores(int curScore, int curTotalScore)
    {
        if(levels[curLevel].name == LevelTypes.Weighing)
        {
            espressoScore.weightScore = curScore;
            espressoScore.weightScoreTotal = curTotalScore;
        }
        else if (levels[curLevel].name == LevelTypes.Reservoir)
        {
            espressoScore.reservoirScore = curScore;
            espressoScore.reservoirScoreTotal = curTotalScore;
        }
        else if (levels[curLevel].name == LevelTypes.PowerOn)
        {
            espressoScore.powerOnScore = curScore;
            espressoScore.powerOnScoreTotal = curTotalScore;
        }
        else if (levels[curLevel].name == LevelTypes.Grind)
        {
            espressoScore.grindScore = curScore;
            espressoScore.grindScoreTotal = curTotalScore;
        }
        else if (levels[curLevel].name == LevelTypes.Tamp)
        {
            espressoScore.tampScore = curScore;
            espressoScore.tampScoreTotal = curTotalScore;
        }
    }
}
