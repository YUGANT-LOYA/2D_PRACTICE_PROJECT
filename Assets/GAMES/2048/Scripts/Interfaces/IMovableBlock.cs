using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovableBlock
{ 
    int Value { get; set; }
    void SetBlock(INode node);
}
