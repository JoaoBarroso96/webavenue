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

    //Data Variables
    public Dictionary<string, Node> dicNodesLvl;

    private float counterHold;
    private GameObject nodeSelected;
    private bool canMoveNode;
    Vector2 mousePos;

    private bool isFinished; // Time ended or user finish

    private int nodesMoved;

    // Start is called before the first frame update
    void Start()
    {
        isFinished = false;
        nodesMoved = 0;
        dicNodesLvl = new Dictionary<string, Node>();
        createUI.CreateLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFinished)
        {
            //PC
            if (isPC)
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

                    if (counterHold > timeHold && nodeSelected != null)
                    {
                        canMoveNode = true;
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

            // Handle screen touches.
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                counterHold += Time.deltaTime;
                print(counterHold);
                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.tag == "Node")
                        {
                            print("a");
                        }
                    }
                }
                // Move the cube if the screen has the finger moving.
                if (touch.phase == TouchPhase.Moved)
                {
                    /*Vector2 pos = touch.position;
                    pos.x = (pos.x - width) / width;
                    pos.y = (pos.y - height) / height;
                    position = new Vector3(-pos.x, pos.y, 0.0f);

                    // Position the cube.
                    transform.position = position;*/
                }

                /*if (Input.touchCount == 2)
                {
                    touch = Input.GetTouch(1);

                    if (touch.phase == TouchPhase.Began)
                    {
                        // Halve the size of the cube.
                        transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                    }

                    if (touch.phase == TouchPhase.Ended)
                    {
                        // Restore the regular size of the cube.
                        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    }
                }*/

                if (touch.phase == TouchPhase.Ended)
                {

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
                print("win");
            }
        }
    }
}
