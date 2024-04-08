using System;
using UnityEngine;

namespace YugantLoyaLibrary.SudokuSolver
{
    public class SudokuBox : MonoBehaviour
    {
        public Vector2Int sudokuBoxId;
        [SerializeField] private SudokuTile[] totalTilesInBoxArr;
        [SerializeField] private TileStruct[] horizontalTileStructArr,verticalTileStructArr;

        [Serializable]
        public struct TileStruct
        {
            public SudokuTile[] sudokuTileArr;
        }
        
        public SudokuTile[] GetAllBoxTiles()
        {
            return totalTilesInBoxArr;
        }

        public SudokuTile[] GetHorizontalLineTiles(int rowNum)
        {
            return horizontalTileStructArr[rowNum].sudokuTileArr;
        }

        public SudokuTile[] GetVerticalLineTiles(int colNum)
        {
            return verticalTileStructArr[colNum].sudokuTileArr;
        }
    }
}