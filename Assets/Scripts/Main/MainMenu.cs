using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    public void OnClickPlay()
    {
        soundManager.PlayClip("tap");
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void OnClickLevels()
    {
        soundManager.PlayClip("tap");
        SceneManager.LoadScene("Levels", LoadSceneMode.Single);
    }
}
