using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YugantLoyaLibrary.Game2048
{
    public class Node : MonoBehaviour
    {
        public Vector2 Pos => transform.position;
        public Block occupiedBlock;

    }
}
