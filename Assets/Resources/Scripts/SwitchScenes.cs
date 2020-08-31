using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Author: Noah Logan

public class SwitchScenes : MonoBehaviour
{
    private float switchTime;
    private float switchTimer;

    void Start()
    {
        switchTime = 0;
        switchTimer = 5;
    }

    void Update()
    {
        switchTime += Time.deltaTime;

        if (switchTime > switchTimer)
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            switchTime = 0;
        }
    }
}
