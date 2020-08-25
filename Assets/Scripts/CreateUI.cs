using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateUI : MonoBehaviour
{
    public Level level;

    public Vector2 gridSize;
    public Vector2 startPosition;
    public int nodeSize;
    public GameObject goGrid;
    public GameObject goNode;

    public GameLogic gameLogic;


    public void CreateLevel()
    {
        for (int c = 0; c < level.nodes.Count; c++)
        {
            int i = Mathf.RoundToInt(level.nodes[c].startPosition.y);
            int j = Mathf.RoundToInt(level.nodes[c].startPosition.x);

            GameObject newNode = Instantiate(goNode, goGrid.transform);
            newNode.transform.localPosition = new Vector3(startPosition.x + nodeSize * j, startPosition.y - nodeSize * i, 0);

            Node n = newNode.GetComponent<Node>();
            n.SetCorrectPosition(Mathf.RoundToInt(level.nodes[c].correctPosition.x),Mathf.RoundToInt(level.nodes[c].correctPosition.y));
            n.SetCanMove(level.nodes[c].canMove);
            n.SetGameLogic(gameLogic);
            n.SetGoGrid(goGrid);
            n.SetCreateUI(this);
            n.SetPosition(j, i);

            //REMOVER
            //newNode.GetComponent<Image>().color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            newNode.GetComponent<Image>().sprite = level.nodes[c].image;

            gameLogic.dicNodesLvl.Add(j + "_" + i, n);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        /*for (int i = 0; i < gridSize.y; i++)
        {
            for (int j = 0; j < gridSize.x; j++)
            {
                GameObject newNode = Instantiate(goNode, goGrid.transform);
                newNode.transform.localPosition = new Vector3(startPosition.x + nodeSize * j, startPosition.y - nodeSize * i, 0);

                Node n = newNode.GetComponent<Node>();
                n.SetGoGrid(goGrid);
                n.SetCreateUI(this);
                n.SetPosition(j, i);

                //REMOVER
                newNode.GetComponent<Image>().color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

                gameLogic.dicNodesLvl.Add(j + "_" + i, n);
            }
        }*/

        

        

    }

}
