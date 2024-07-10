using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YugantLoyaLibrary.Game_2048
{
    public class GridBase : MonoBehaviour
    {
        public Vector3 Pos
        {
            get => transform.position;
            set => transform.position = value;
        }

        public BlockBase OccupiedBlockBase { get; set; }


        void Init()
        {
            
        }
    }
}