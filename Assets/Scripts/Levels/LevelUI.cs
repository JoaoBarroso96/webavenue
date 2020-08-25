using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    public GameObject goAreaLevels;
    public GameObject goBtnLock;
    public GameObject goBtnUnLock;
    public Vector2 startPosition;
    public Vector2 padding;
    public Color starWin;

    private int maxLine = 4;

    // Start is called before the first frame update
    void Start()
    {
        int userLvl = PlayerPrefs.GetInt("userLevel", 1);
        userLvl = 3;
        for (int i = 0; i < Global.MAX_LEVEL; i++)
        {
            int lvl = i + 1;
            GameObject goBtn = null;
            if (i < userLvl) // Check unlock lvl
            {
                goBtn = Instantiate(goBtnUnLock);
                goBtn.transform.GetChild(3).GetComponent<Text>().text = (lvl).ToString();

                //Stars
                int lvlStar = PlayerPrefs.GetInt("lvl_" + lvl, 0);
                //lvlStar = 3;
                for (int j = 0; j < lvlStar; j++)
                {
                    goBtn.transform.GetChild(j).GetComponent<Image>().color = starWin;
                }
                goBtn.GetComponent<Button>().onClick.RemoveAllListeners();
                goBtn.GetComponent<Button>().onClick.AddListener(() => OpenLvl(lvl));
            }
            else
            {
                goBtn = Instantiate(goBtnLock);
            }

            //Position
            goBtn.transform.SetParent(goAreaLevels.transform);
            goBtn.transform.localScale = new Vector3(1f, 1f, 1f);
            int x = i % maxLine;
            int y = (int)Mathf.Ceil(i / maxLine);
            goBtn.transform.localPosition = new Vector3(startPosition.x + padding.x * x, startPosition.y - padding.y * y, 0);

            
        }
    }

    public void OnClickMain()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void OpenLvl(int id)
    {
        print("aaa");
        PlayerPrefs.SetInt("currentLvl", id);
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
