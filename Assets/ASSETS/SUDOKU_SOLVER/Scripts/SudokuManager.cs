using System;
using UnityEngine;

namespace YugantLoyaLibrary.SudokuSolver
{
    public class SudokuManager : MonoBehaviour
    {
        public static Action<SudokuTile> selectedTileEvent;
        public static SudokuTile currSudokuTile, lastSudokuTile;

        private void OnEnable()
        {
            selectedTileEvent += CurrentSelectedTile;
        }

        private void OnDisable()
        {
            selectedTileEvent -= CurrentSelectedTile;
        }

        void CurrentSelectedTile(SudokuTile tile)
        {
            //Assign Last Sudoku Tile as previous Tile Selected by user.
            lastSudokuTile = currSudokuTile;
            //Update Current Selected Tile as currSudoku Tile.
            currSudokuTile = tile;

            Debug.Log("Curr Tile : "+ currSudokuTile.gameObject.name);
            
            //Condition : Only when we start selecting tile for the first time, when lastSudoku Tile is Empty.
            if (lastSudokuTile == null)
                return;

            Debug.Log("Last Tile : "+ lastSudokuTile.gameObject.name);
            
            lastSudokuTile.OnTileDeselect();
            
            
        }
    }
}