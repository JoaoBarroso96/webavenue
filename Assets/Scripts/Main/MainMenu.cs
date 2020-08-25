﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnClickPlay()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void OnClickLevels()
    {
        SceneManager.LoadScene("Levels", LoadSceneMode.Single);
    }
}