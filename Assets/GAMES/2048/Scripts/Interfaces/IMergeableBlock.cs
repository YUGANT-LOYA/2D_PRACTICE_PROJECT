using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMergeableBlock
{
    bool IsMergeAllowed { get; set; }
    IMergeableBlock MergeBlockWith { get; set; }
    bool CanMerge(int val);
    void MergeBlock(IMergeableBlock mergeBlockWith);
}