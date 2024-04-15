using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace YugantLoyaLibrary.SudokuSolver
{
    public class SudokuSolver : MonoBehaviour
    {
        
        [Button]
        public void FillSudoku()
        {
            int[][] sudokuDataArr = SudokuManager.instance.GetAllDataOfSudokuTiles();

            
        }
    }
}