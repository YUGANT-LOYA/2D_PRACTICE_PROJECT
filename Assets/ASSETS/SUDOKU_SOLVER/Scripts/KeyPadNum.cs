using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YugantLoyaLibrary.SudokuSolver
{
    public class KeyPadNum : MonoBehaviour
    {
        Action<int> _onKeyPadValueChange;
        public int keyNum;
        public bool isClickAllowed = true;
        [SerializeField] TextMeshProUGUI keyPadText;
        [SerializeField] private Button keyButton;

        public int KeyNum
        {
            get => keyNum;
            set => _onKeyPadValueChange?.Invoke(value);
        }

        private void OnEnable()
        {
            _onKeyPadValueChange += OnValueChanged;
        }

        private void OnDisable()
        {
            _onKeyPadValueChange -= OnValueChanged;
        }

        private void Start()
        {
            KeyNum = keyNum;
        }

        void OnValueChanged(int val)
        {
            keyNum = val;
            keyPadText.text = val.ToString();
        }

        public void OnKeyClicked()
        {
            if (SudokuManager.currSudokuTile == null)
                return;

            if (!SudokuManager.currSudokuTile.canBeChanged)
                return;

            SudokuManager.currSudokuTile.TileVal = KeyNum;

            bool conditionStatus = false;
            SudokuManager.checkConditionEvent?.Invoke(SudokuManager.currSudokuTile, out conditionStatus);

            if (!conditionStatus)
            {
                SudokuManager.currSudokuTile.TileVal = 0;
                //Update the NumberPad Keys
                SudokuManager.instance.UpdateNumberPadKeys();
            }
        }

        public void SetButtonStatus(bool isActive)
        {
            keyButton.interactable = isActive;
            isClickAllowed = isActive;
        }
    }
}