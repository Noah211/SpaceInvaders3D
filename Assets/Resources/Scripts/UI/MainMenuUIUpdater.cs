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
        string[] fileArray = Directory.GetFiles(Constants.SAVED_DATA, "*.xml");

        if (fileArray.Length > 0)
        {
            highScore = StatLoader.LoadHighScore();
        }
        else
        {
            highScore = 0;
        }

        score = 0;
        ScoreCountText.text = score.ToString();
        HighScoreCountText.text = highScore.ToString();
    }

    void Update()
    {

    }
}
