using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YugantLoyaLibrary.Game2048
{
    public class Node : MonoBehaviour
    {
        public Vector2Int ID { get; set; }

        public Vector3 Pos
        {
            get => transform.position;
            set => transform.position = value;
        }

        public Block OccupiedNumberBlock { get; set; }
    }
}