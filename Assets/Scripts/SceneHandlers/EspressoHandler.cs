using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EspressoHandler : MonoBehaviour
{
    public GameObject weighingLevelPrefab;
    public GameObject reservoirLevelPrefab;
    public GameObject powerOnLevelPrefab;
    public GameObject grindLevelPrefab;
    public GameObject tampLevelPrefab;
    public GameObject brewLevelPrefab;
    public GameObject serveLevelPrefab;

    private EspressoScore espressoScore;
    private int curLevel = -1;

    private class Level
    {
        //string name;
        public GameObject prefab;
        public GameObject instance;
        public int score;
        public int scoreTotal;
    }

    private Level[] levels;

    // Start is called before the first frame update
    void Start()
    {
        levels = new Level[]
        {
            new Level
            {
                prefab = weighingLevelPrefab
            }
        };

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
    }

    void StopLevel()
    {
        //save score 

        //destroy level

        //move level
    }

}
