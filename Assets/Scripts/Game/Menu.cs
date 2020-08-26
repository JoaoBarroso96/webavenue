using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject goWinMenu;
    public Animator menuAnimator;
    public Text lblLevel;
    public float coldown = 1.5f; // time to open next lvl
    public float timeStars = 0.7f; // Time to show stars
    public List<Image> lstStars;
    public Sprite winStar;
    public GameObject goTutorial;
    private SoundManager soundManager;
    private bool startTime;
    private bool isOpen; // menu is Open
    private bool canOpenMenu = true;
    private int currentLvl;
    private float timer;
    

    // Start is called before the first frame update
    void Start()
    {
        currentLvl = PlayerPrefs.GetInt("currentLvl");
        lblLevel.text = "#" + currentLvl;
        isOpen = false;
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        goTutorial.SetActive(currentLvl == 1);
    }

    private void Update()
    {
        if (startTime)
        {
            timer += Time.deltaTime;
            if (timer >= coldown)
            {
                PlayerPrefs.SetInt("currentLvl", currentLvl);
                OnClickRestart();
            }
        }
    }

    public void OnClickMenu()
    {
        if (canOpenMenu)
        {
            startTime = false;
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
        
    }

    public void OnClickRestart()
    {
        soundManager.PlayClip("tap");
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void NextLevel()
    {
        currentLvl++;
        if (currentLvl > Global.MAX_LEVEL)
        {
            currentLvl = Global.MAX_LEVEL;
        }
        PlayerPrefs.SetInt("currentLvl", currentLvl);
        soundManager.PlayClip("tap");
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void OnClickLevels()
    {
        soundManager.PlayClip("tap");
        SceneManager.LoadScene("Levels", LoadSceneMode.Single);
    }

    public void OnClickNextLvl()
    {
        if (currentLvl < PlayerPrefs.GetInt("userLevel"))
        {
            soundManager.PlayClip("tap");
            timer = 0f;
            startTime = true;
            currentLvl++;
            lblLevel.text = "#" + currentLvl;
        }
    }

    public void OnClickBackLvl()
    {
        if (currentLvl > 1)
        {
            soundManager.PlayClip("tap");
            timer = 0f;
            startTime = true;
            currentLvl--;
            lblLevel.text = "#" + currentLvl;
        }
    }

    public void ShowWinMenu(int nStars)
    {
        canOpenMenu = false;
        goWinMenu.SetActive(true);
        goWinMenu.transform.GetChild(3).GetComponent<Text>().text = "Level " + currentLvl;
        Vector3 newPosition = new Vector3(0f, 0f, 0f);
        soundManager.PlayClip("applause");
        StartCoroutine(WinAnimation(goWinMenu.transform, goWinMenu.transform.localPosition, newPosition, 1000f, nStars));
    }

    IEnumerator WinAnimation(Transform objectToMove, Vector3 a, Vector3 b, float speed, int nStars)
    {
        float step = (speed / (a - b).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f)
        {
            t += step; // Goes from 0 to 1, incrementing by step each time
            objectToMove.localPosition = Vector3.Lerp(a, b, t); // Move objectToMove closer to b
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }
        objectToMove.localPosition = b;

        //Show stars
        yield return new WaitForSeconds(timeStars);
        for(int i = 0; i < nStars; i++)
        {
            lstStars[i].sprite = winStar;
            yield return new WaitForSeconds(timeStars);
        }

        goWinMenu.transform.GetChild(1).gameObject.SetActive(true);
        goWinMenu.transform.GetChild(2).gameObject.SetActive(true);

    }

}
