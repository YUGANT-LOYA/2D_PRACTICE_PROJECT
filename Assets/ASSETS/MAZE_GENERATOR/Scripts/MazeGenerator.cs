using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace YugantLoyaLibrary.MazeGenerator
{
    public class MazeGenerator : MonoBehaviour
    {
        public GameObject gridPrefab;
        public RectTransform gridContainerTrans;
        [SerializeField] GridLayoutGroup containerGridLayoutGroup;
        public Vector2Int gridSize = new Vector2Int(5,5);


        private void Start()
        {
            CreateGrid();
        }

        private void CreateGrid()
        {
            for (int i = 0; i < gridSize.y; i++)
            {
                for (int j = 0; j < gridSize.x; j++)
                {
                    GameObject grid = Instantiate(gridPrefab, gridContainerTrans);
                    GridTile gridTile = grid.GetComponent<GridTile>();
                    gridTile.gridID = new Vector2Int(i, j);
                    gridTile.name = $"Grid_{i}_{j}";
                }
            }
            
            containerGridLayoutGroup.constraintCount = gridSize.y;
        }
    }
}
