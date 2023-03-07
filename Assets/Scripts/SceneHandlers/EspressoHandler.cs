using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EspressoHandler : MonoBehaviour
{
    [System.Serializable]
    public class Level
    {
        public string name;
        public GameObject prefab;
        [System.NonSerialized]
        public GameObject instance;
        [System.NonSerialized]
        public int score;
        [System.NonSerialized]
        public int scoreTotal;
    }

    private EspressoScore espressoScore;
    private int curLevel = -1;

    [LabeledArrayAttribute(new string[] { "Weighing", "Reservoir", "Power On", "Grind", "Tamp", "Brew", "Serve"})]
    public Level[] levels = new Level[7];

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
        levels[curLevel].instance = Instantiate(levels[curLevel].prefab);
        levels[curLevel].score = 10;
        levels[curLevel].scoreTotal = 15;
        //Debug.Log(espressoScore.weightScore);
        //Debug.Log(espressoScore.weightScoreTotal);
    }

    void StopLevel()
    {
        //save score 

        //destroy level

        //move level
    }

}
