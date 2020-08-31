using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

// Author: Noah Logan

public class GameUIUpdater : MonoBehaviour
{
    public Text ScoreCountText;
    public Text HighScoreCountText;
    public Text LivesText;
    public Text WaveText;
    public Text GameOverText;

    private int score;
    private int highScore;
    private int lives;
    private int wave;

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
        lives = 3;
        wave = 1;
        ScoreCountText.text = score.ToString();
        HighScoreCountText.text = highScore.ToString();
        LivesText.text = "Lives: " + lives.ToString();
        WaveText.text = "Wave: " + wave.ToString();
        GameOverText.text = "";
    }

    void Update()
    {

    }

    public void UpdateScore(int pointValueToAdd)
    {
        score += pointValueToAdd;
        ScoreCountText.text = score.ToString();

        if (score > highScore)
        {
            highScore = score;
            HighScoreCountText.text = highScore.ToString();
        }
    }

    public void UpdateLives(int lives)
    {
        this.lives = lives;
        LivesText.text = "Lives: " + lives.ToString();

        if (lives == 0)
        {
            DisplayGameOver();
        }
    }

    public void UpdateWave()
    {
        wave++;
        WaveText.text = "Wave: " + wave.ToString();

        if (score >= highScore)
        {
            StatSaver.SaveStats(highScore);
        }
    }

    public void DisplayGameOver()
    {
        GameOverText.text = "Game Over";

        if (score >= highScore)
        {
            StatSaver.SaveStats(highScore);
        }
    }
}
