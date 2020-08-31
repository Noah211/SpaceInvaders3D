using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Author: Noah Logan

public class MainMenuController : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }
    }
}
