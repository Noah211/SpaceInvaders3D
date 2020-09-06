using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

// Author: Noah Logan

public class MainMenuUIUpdater : MonoBehaviour
{
    public Text ScoreCountText;
    public Text HighScoreCountText;

    private int score;
    private int highScore;

    void Start()
    {
        highScore = StatLoader.LoadHighScore();
        score = 0;
        ScoreCountText.text = score.ToString();
        HighScoreCountText.text = highScore.ToString();
    }

    void Update()
    {

    }
}
