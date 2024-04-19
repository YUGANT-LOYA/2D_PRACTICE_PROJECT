using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace YugantLoyaLibrary.SudokuSolver
{
    public class SudokuUI : MonoBehaviour
    {
        public static SudokuUI instance;

        public enum ButtonType
        {
            None,
            DefaultButtonSet,
            SlowSolutionButtonSet,
            CreateBoardButtonSet,
        }

        [Serializable]
        public struct SudokuButtons
        {
            public ButtonType buttonType;
            public List<UIButtonVisibleStruct> uiButtonVisibleStructList;
        }

        [Serializable]
        public struct UIButtonVisibleStruct
        {
            public GameObject gmObj;
            public bool isActive;
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

        public void SetButtonStatusOfButtonType(ButtonType buttonType)
        {
            foreach (SudokuButtons sudokuButtons in sudokuButtonsList)
            {
                if (sudokuButtons.buttonType == buttonType)
                {
                    foreach (UIButtonVisibleStruct button in sudokuButtons.uiButtonVisibleStructList)
                    {
                        button.gmObj.SetActive(button.isActive);
                    }
                    
                    return;
                }
            }
        }
    }
}