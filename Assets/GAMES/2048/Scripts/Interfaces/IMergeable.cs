using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YugantLoyaLibrary.Game_2048
{
    public interface IMergeable
    {
        bool IsMergeAllowed { get; set; }
        BlockBase MergeBlockWith { get; set; }
        bool CanMerge(int val);
        void MergeBlock(BlockBase mergeBlockWith);
    }
}