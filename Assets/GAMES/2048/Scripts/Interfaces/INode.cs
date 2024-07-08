using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INode
{
    public Vector2Int ID { get; set; }
    public Vector3 Pos { get; set; }
    public INumberBlock OccupiedNumberBlock { get; set; }
}