using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public float timeHold;
    public bool isPC;
    public GameObject goGrid;
    public GameObject goGameArea;
    public CreateUI createUI;
    public Menu menu;

    //Data Variables
    public Dictionary<string, Node> dicNodesLvl;
    public List<int> thresholdStar; // 3 stars ; 2 stars; 1 star
    private float timerLevel;

    private float counterHold; //Time hold to change node
    private GameObject nodeSelected;
    private bool canMoveNode;
    Vector2 mousePos;

    private bool isFinished; // Time ended or user finish

    private int nodesMoved;
    private int currentLvl;

    private Level level;
    private SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        currentLvl = PlayerPrefs.GetInt("currentLvl", 1);
        isFinished = false;
        nodesMoved = 0;
        timerLevel = 0f;
        dicNodesLvl = new Dictionary<string, Node>();
        level = Resources.Load<Level>("Levels/Level" + currentLvl);
        createUI.CreateLevel(level);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFinished)
        {
            timerLevel += Time.deltaTime;

            if (!menu.MenuIsOpen())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, -Vector2.up);
                    if (hit.collider != null)
                    {
                        //Verify select node
                        if (hit.transform.tag == "Node")
                        {
                            Node ns = hit.transform.GetComponent<Node>();
                            if (ns.NodeCanMove())
                            {
                                nodeSelected = hit.transform.gameObject;
                                nodeSelected.transform.SetParent(goGameArea.transform);
                            }
                        }

                    }

                }

                if (Input.GetMouseButton(0))
                {
                    counterHold += Time.deltaTime;

                    if (counterHold > timeHold && nodeSelected != null && !canMoveNode)
                    {
                        canMoveNode = true;
                        soundManager.PlayClip("nodeSelect");
                    }

                    if (canMoveNode)
                    {
                        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        nodeSelected.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1f);
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {
                    if (nodeSelected != null)
                    {
                        nodeSelected.transform.SetParent(goGrid.transform);

                        int gridX = Mathf.RoundToInt((nodeSelected.transform.localPosition.x - createUI.startPosition.x) / createUI.nodeSize);
                        int gridY = Mathf.RoundToInt((createUI.startPosition.y - nodeSelected.transform.localPosition.y) / createUI.nodeSize);

                        if (gridX >= 0 && gridX < createUI.gridSize.x && gridY >= 0 && gridY < createUI.gridSize.y)//Valid position
                        {

                            string house = gridX + "_" + gridY;
                            if (dicNodesLvl.ContainsKey(house)) //Check has node in position
                            {
                                if (dicNodesLvl[house].NodeCanMove())
                                {
                                    //Change position
                                    Vector2 oldPosition = nodeSelected.GetComponent<Node>().GetPosition();
                                    int oldX = Mathf.RoundToInt(oldPosition.x);
                                    int oldY = Mathf.RoundToInt(oldPosition.y);

                                    //Selected Node
                                    Node aux = dicNodesLvl[house];
                                    dicNodesLvl[house] = nodeSelected.GetComponent<Node>();
                                    dicNodesLvl[house].SetPosition(gridX, gridY);

                                    //Old Node
                                    aux.SetPosition(oldX, oldY);
                                    dicNodesLvl[oldX + "_" + oldY] = aux;

                                    dicNodesLvl[oldX + "_" + oldY].transform.SetParent(goGameArea.transform);
                                    dicNodesLvl[house].transform.SetParent(goGameArea.transform);
                                    dicNodesLvl[oldX + "_" + oldY].MoveToInit();
                                    dicNodesLvl[house].MoveToInit();

                                    nodesMoved = 2; //How many node is moved
                                }
                                else
                                {
                                    nodeSelected.GetComponent<Node>().MoveToInit();
                                }
                                
                            }
                            else
                            {
                                nodeSelected.GetComponent<Node>().MoveToInit();
                            }
                        }
                        else
                        {
                            nodeSelected.GetComponent<Node>().MoveToInit();
                        }


                    }
                    counterHold = 0;
                    nodeSelected = null;
                    canMoveNode = false;
                }
            }
            
        }
    }


    public void CheckUserWin()
    {
        nodesMoved--;
        if (nodesMoved == 0)
        {
            bool userWin = true;
            //Iterate nodes dictionary
            foreach (KeyValuePair<string, Node> node in dicNodesLvl)
            {
                if (!node.Value.IsPositionedCorrect())
                {
                    userWin = false;
                    break;
                }
            }

            if (userWin) // User Won Level
            {
                isFinished = true;

                //Calculate Pontuation
                
                int lvlStar = 0;
                if (timerLevel < level.time)
                {
                    float p = 100 * timerLevel / level.time;
                    if (p <= thresholdStar[0])
                    {
                        lvlStar = 3;
                    }
                    else if (p <= thresholdStar[1])
                    {
                        lvlStar = 2;
                    }
                    else
                    {
                        lvlStar = 1;
                    }
                }
                 
                
                if (PlayerPrefs.GetInt("lvl_" + currentLvl, 0) < lvlStar)
                {
                    PlayerPrefs.SetInt("lvl_" + currentLvl, lvlStar);
                }

                menu.ShowWinMenu(lvlStar);

                //Update PlayerLevel
                int nextLvl = currentLvl + 1;
                if (nextLvl > PlayerPrefs.GetInt("userLevel", 0))
                {
                    if (nextLvl > Global.MAX_LEVEL)
                    {
                        nextLvl = currentLvl;
                    }
                    PlayerPrefs.SetInt("userLevel", nextLvl);
                }
            }
        }
    }
}
