using System;
using TMPro;
using UnityEngine;

namespace YugantLoyaLibrary.SudokuSolver
{
    public class KeyPadNum : MonoBehaviour
    {
        Action<int> _onKeyPadValueChange;
        public int keyNum;
        [SerializeField] TextMeshProUGUI keyPadText;

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
            //Debug.Log("Key Clicked : " + KeyNum);
            
            if (SudokuManager.currSudokuTile != null)
            {
                SudokuManager.currSudokuTile.TileVal = KeyNum;

            }
        }
    }
}