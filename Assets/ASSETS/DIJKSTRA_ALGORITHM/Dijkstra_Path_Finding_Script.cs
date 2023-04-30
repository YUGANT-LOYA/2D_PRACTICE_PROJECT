using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Yugant_Library
{
    public class Dijkstra_Path_Finding_Script : MonoBehaviour
    {
        public GameObject sourceNode, destinationNode;

        [SerializeField] List<GameObject> totalNodeList;
        public List<GameObject> visitedNodeList, unVisitedNodeList;
        int counter = 0;

        [Button]
        public void DijkstraAlgo()
        {
            unVisitedNodeList.Clear();

            foreach (GameObject gm in totalNodeList)
            {
                unVisitedNodeList.Add(gm);
            }

            while (unVisitedNodeList.Count <= 0)
            {
                Dijkstra_Node_Script startNode = unVisitedNodeList[counter].GetComponent<Yugant_Library.Dijkstra_Node_Script>();

                for(int i = 0; i < startNode.connectedNodeList.Count; i++)
                {
                    if(!startNode.connectedNodeList[i].isVisited)
                    {

                    }
                }
            }
        }

        Dijkstra_Node_Script PickNextNode(Dijkstra_Node_Script start)
        {
           for(int i=0;i < start.connectedNodeList.Count;i++)
            {
                if(start.connectedNodeList[i].isVisited == false)
                {
                    
                }
            }

            return start;
        }

        void ShortestPath(GameObject start, GameObject end)
        {

        }
    }
}
