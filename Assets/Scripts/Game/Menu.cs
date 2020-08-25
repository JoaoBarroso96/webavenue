using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Animator menuAnimator;
    private SoundManager soundManager;
    private bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    public void OnClickMenu()
    {
        soundManager.PlayClip("tap");
        if (isOpen)
        {
            menuAnimator.SetTrigger("Close");
        }
        else
        {
            menuAnimator.SetTrigger("Open");
        }
        

        isOpen = !isOpen;
    }

    public void OnClickRestart()
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
