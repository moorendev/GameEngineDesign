using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int score;

    void Awake()
    {
        if(!instance)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScore(int boxValue)
    {
        score += boxValue;
        UnityEngine.Debug.Log(score);
    }

    public void DecreaseScore()
    {
        score -= 1;
        UnityEngine.Debug.Log(score);
    }
}
