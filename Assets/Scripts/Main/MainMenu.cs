﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        int userLvl = PlayerPrefs.GetInt("userLevel", 0);
        if (userLvl == 0)
        {
            PlayerPrefs.SetInt("userLevel", 1);
        }
        //Remove
       //PlayerPrefs.SetInt("userLevel", 1);
    }

    public void OnClickPlay()
    {
        PlayerPrefs.SetInt("currentLvl", PlayerPrefs.GetInt("userLevel", 1));
        soundManager.PlayClip("tap");
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void OnClickLevels()
    {
        soundManager.PlayClip("tap");
        SceneManager.LoadScene("Levels", LoadSceneMode.Single);
    }
}
