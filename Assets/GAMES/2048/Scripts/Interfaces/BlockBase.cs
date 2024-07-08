using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BlockBase : MonoBehaviour
{
    public Vector3 Pos { get; set; }
    public INode CurrNode { get; set; }
}
