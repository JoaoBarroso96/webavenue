using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Node", menuName = "Nodes")]
public class NodeTemplate : ScriptableObject
{
    public Vector2 startPosition;
    public Vector2 correctPosition;
    public Sprite image;
    public enum typeNode { Wire, Lamp , Power };
    public typeNode sampleVariable;
}
