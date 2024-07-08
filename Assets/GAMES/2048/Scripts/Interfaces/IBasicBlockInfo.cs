using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBasicBlockInfo
{
    Vector3 Pos { get; set; }
    INode CurrNode { get; set; }
}