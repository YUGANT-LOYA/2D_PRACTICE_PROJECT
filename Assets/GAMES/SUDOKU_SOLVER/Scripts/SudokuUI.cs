using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YugantLoyaLibrary.SudokuSolver
{
    public class SudokuUI : MonoBehaviour
    {
        public static SudokuUI instance;

        public enum ButtonType
        {
            Generate,
            ClearCurrentSudoku,
            EmptySudokuBoard,
            FastSolution,
            SlowSolution,
            Cancel,
            DifficultyButton
        }

        [Serializable]
        public struct SudokuButtons
        {
            public ButtonType buttonType;
            public GameObject button;
        }

        public List<SudokuButtons> sudokuButtonsList;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void SlowSolutionRunning()
        {
            foreach (SudokuButtons sudokuButtons in sudokuButtonsList)
            {
                if (sudokuButtons.buttonType != ButtonType.Cancel)
                {
                    sudokuButtons.button.gameObject.SetActive(false);
                }
                else if(sudokuButtons.buttonType == ButtonType.Cancel)
                {
                    sudokuButtons.button.gameObject.SetActive(true);
                }
            }
            
            NumberPadManager.instance.VisibilityStatusOfAllKeys(false);
        }

        public void ResetAllButtons()
        {
            foreach (SudokuButtons sudokuButtons in sudokuButtonsList)
            {
                if (sudokuButtons.buttonType != ButtonType.Cancel)
                {
                    sudokuButtons.button.gameObject.SetActive(true);
                }
                else if(sudokuButtons.buttonType == ButtonType.Cancel)
                {
                    sudokuButtons.button.gameObject.SetActive(false);
                }
            }
        }
        
    }
}