using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YugantLoyaLibrary.SudokuSolver
{
    public class SudokuTile : MonoBehaviour
    {
        Action<int> _onTileValueChangeEvent;
        public int tileVal;
        public bool canBeChanged;
        [SerializeField] private TextMeshProUGUI tileText;
        [SerializeField] private Outline tileOutline;
        public Color defaultTileOutlineColor, highlightTileOutlineColor;

        public int TileVal
        {
            get => tileVal;
            set => _onTileValueChangeEvent?.Invoke(value);
        }

        private void OnEnable()
        {
            _onTileValueChangeEvent += OnSudokuTileValueChange;
        }

        private void OnDisable()
        {
            _onTileValueChangeEvent -= OnSudokuTileValueChange;
        }

        private void Start()
        {
            TileVal = tileVal;
        }

        private void Update()
        {
        }

        void OnSudokuTileValueChange(int val)
        {
            if (!canBeChanged)
                return;

            tileVal = val;
            tileText.text = val is <= 9 and >= 1 ? tileVal.ToString() : "";
        }

        public void OnTileSelect()
        {
            Debug.Log("Tile Selected : " + gameObject.name);
            SudokuManager.selectedTileEvent?.Invoke(this);
            HighlightTile();
        }

        public void OnTileDeselect()
        {
            Debug.Log("Tile Deselected : " + gameObject.name);
            //SudokuManager.selectedTileEvent?.Invoke(null);
            SetDefaultTileColor();
        }

        void HighlightTile()
        {
            tileOutline.effectColor = highlightTileOutlineColor;
        }

        void SetDefaultTileColor()
        {
            tileOutline.effectColor = defaultTileOutlineColor;
        }
    }
}