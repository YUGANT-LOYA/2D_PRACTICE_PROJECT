using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace YugantLoyaLibrary.MazeGenerator
{
    public class GridTile : MonoBehaviour
    {
        public enum GridSide
        {
            Left,
            Right,
            Top,
            Bottom
        }

        public enum GridNeighbour
        {
            Left,
            Right,
            Top,
            Bottom
        }

        [System.Serializable]
        public struct GridSideStruct
        {
            public GridSide side;
            public GameObject sideGm;
        }
        
        [System.Serializable]
        public struct GridNeighbourStruct
        {
            public GridNeighbour gridNeighbour;
            public GameObject neighbourGm;
        }
        
        public Vector2Int gridID;
        public bool isVisited;
        public GameObject gridBg;
        public List<GridSideStruct> gridSideStructList;
        public List<GridNeighbourStruct> gridNeighbourStructList;


        
        
        
        [Button]
        public void RemoveSide(GridSide gridSide)
        {
            foreach (GridSideStruct gridSideStruct in gridSideStructList)
            {
                if (gridSideStruct.side == gridSide)
                {
                    gridSideStruct.sideGm.SetActive(false);
                    break;
                }
            }
        }

        [Button]
        public void ResetGridSides()
        {
            foreach (GridSideStruct gridSideStruct in gridSideStructList)
            {
                gridSideStruct.sideGm.SetActive(true);
            }
        }
    }
}