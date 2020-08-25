using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Levels")]
public class Level : ScriptableObject
{
    public float time;
    public List<NodeTemplate> nodes;
}
