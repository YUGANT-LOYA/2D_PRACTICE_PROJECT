using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yugant_Library
{


    public class Dijkstra_Node_Script : MonoBehaviour
    {
        [System.Serializable]
        public struct ConnectionNodeStruct
        {
            public GameObject connectedNode;
            public float nodeWeight;
            public bool isVisited;
        }

        public List<ConnectionNodeStruct> connectedNodeList;
    }
}