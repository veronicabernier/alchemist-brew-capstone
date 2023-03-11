using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EspressoHandler : MonoBehaviour
{
    [System.Serializable]
    public class Level
    {
        public LevelTypes name;

        public GameObject prefab;

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

    private EspressoScore espressoScore = new EspressoScore();
    private int curLevel = -1;


    // Start is called before the first frame update
    void Start()
    {
        MoveLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MoveLevel()
    {
        curLevel += 1;
        levels[curLevel].instance = Instantiate(levels[curLevel].prefab, transform);
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
            MoveLevel();
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
