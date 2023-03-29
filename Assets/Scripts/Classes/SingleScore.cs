using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleScore
{
    public int curScore;
    public int curScoreTotal;
    public List<string> comments;

    public SingleScore(int curScore, int curScoreTotal, List<string> comments)
    {
        this.curScore = curScore;
        this.curScoreTotal = curScoreTotal;
        this.comments = comments;
    }
}
