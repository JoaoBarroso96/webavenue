using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private CreateUI createUI;
    private Vector2 gridPosition;
    private Vector2 correctPosition;
    private GameObject goGrid;
    private GameLogic gameLogic;
    private bool canMove;

    public void SetCanMove(bool move)
    {
        this.canMove = move;
    }

    public void SetGameLogic(GameLogic gl)
    {
        this.gameLogic = gl;
    }

    public void SetGoGrid(GameObject goGrid)
    {
        this.goGrid = goGrid;
    }

    public void SetCreateUI(CreateUI createUI)
    {
        this.createUI = createUI;
    }

    public void SetPosition(int x, int y)
    {
        gridPosition = new Vector2(x, y);
    }

    public void SetCorrectPosition(int x, int y)
    {
        correctPosition = new Vector2(x, y);
    }

    public bool NodeCanMove()
    {
        return this.canMove;
    }

    public Vector2 GetPosition()
    {
        return gridPosition;
    }
    
    public void MoveToInit()
    {
        Vector3 newPosition = new Vector3(createUI.startPosition.x + createUI.nodeSize * gridPosition.x, createUI.startPosition.y - createUI.nodeSize * gridPosition.y, 0);
        StartCoroutine(MoveFromTo(transform, transform.localPosition, newPosition, 700f));
    }

    public bool IsPositionedCorrect()
    {
        print(gridPosition + " _ " + correctPosition);
        return gridPosition == correctPosition;
    }

    IEnumerator MoveFromTo(Transform objectToMove, Vector3 a, Vector3 b, float speed)
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
        transform.SetParent(goGrid.transform);

        gameLogic.CheckUserWin();
    }
}
