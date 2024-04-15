using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace YugantLoyaLibrary.SudokuSolver
{
    public class SudokuManager : MonoBehaviour
    {
        public static SudokuManager instance;
        public int testingInt;
        public SudokuTile testingTile;
        public static Action<SudokuTile> selectedTileEvent;

        public delegate bool CheckConditionDelegate(SudokuTile tile);

        public static CheckConditionDelegate checkConditionEvent;

        public static SudokuTile currSudokuTile;
        private static SudokuTile _lastSudokuTile;

        public SudokuBox[] totalSudokuBoxesArr;
        [SerializeField] private BoxStruct[] horizontalBoxStructArr, verticalBoxStructArr;

        [Serializable]
        public struct BoxStruct
        {
            public SudokuBox[] sudokuBoxArr;
        }

        private void OnEnable()
        {
            selectedTileEvent += CurrentSelectedTile;
            checkConditionEvent += CheckCondition;
        }

        private void OnDisable()
        {
            selectedTileEvent -= CurrentSelectedTile;
            checkConditionEvent -= CheckCondition;
        }

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

        private void Start()
        {
        }

        public int[][] GetAllDataOfSudokuTiles()
        {
            int[] totalValArr = new int[81];
            int totalValindex = 0;
            foreach (SudokuBox sudokuBox in totalSudokuBoxesArr)
            {
                foreach (SudokuTile sudokuTile in sudokuBox.GetAllBoxTiles())
                {
                    totalValArr[totalValindex] = sudokuTile.TileVal;
                    totalValindex++;
                }
            }


            int[][] data = new int[9][];
            int tempIndex = 0;
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new int[9];

                for (int j = 0; j < data[i].Length; j++)
                {
                    data[i][j] = totalValArr[tempIndex];
                    tempIndex++;
                }
            }

            return data;
        }


        void CurrentSelectedTile(SudokuTile tile)
        {
            //Assign Last Sudoku Tile as previous Tile Selected by user.
            _lastSudokuTile = currSudokuTile;
            //Update Current Selected Tile as currSudoku Tile.
            currSudokuTile = tile;

            //Debug.Log("Curr Tile : " + currSudokuTile.gameObject.name);

            //Condition : Only when we start selecting tile for the first time, when lastSudoku Tile is Empty.
            if (_lastSudokuTile == null)
                return;

            //Debug.Log("Last Tile : " + _lastSudokuTile.gameObject.name);

            _lastSudokuTile.OnTileDeselect();
        }

        public SudokuTile[] GetSudokuRow(int rowNum)
        {
            SudokuTile[] tempTileArr = new SudokuTile[9];
            int boxStructIndex = rowNum / 3;
            int tileStructIndex = rowNum % 3;
            int tempIndex = 0;

            BoxStruct boxStruct = horizontalBoxStructArr[boxStructIndex];

            for (var i = 0; i < boxStruct.sudokuBoxArr.Length; i++)
            {
                SudokuBox box = boxStruct.sudokuBoxArr[i];
                SudokuTile[] boxTileArr = box.GetHorizontalLineTiles(tileStructIndex);

                for (int j = 0; j < boxTileArr.Length; j++)
                {
                    tempTileArr[tempIndex] = boxTileArr[j];
                    tempIndex++;
                }
            }

            return tempTileArr;
        }

        public SudokuTile[] GetSudokuColumn(int colNum)
        {
            SudokuTile[] tempTileArr = new SudokuTile[9];
            int boxStructIndex = colNum / 3;
            int tileStructIndex = colNum % 3;
            int tempIndex = 0;

            BoxStruct boxStruct = verticalBoxStructArr[boxStructIndex];

            for (var i = 0; i < boxStruct.sudokuBoxArr.Length; i++)
            {
                SudokuBox box = boxStruct.sudokuBoxArr[i];
                SudokuTile[] boxTileArr = box.GetVerticalLineTiles(tileStructIndex);

                for (int j = 0; j < boxTileArr.Length; j++)
                {
                    tempTileArr[tempIndex] = boxTileArr[j];
                    tempIndex++;
                }
            }

            return tempTileArr;
        }


        public bool CheckCondition(SudokuTile currTile)
        {
            Debug.Log($"Check Condition Entered !");

            // Debug.Log(
            //     $"Check Condition Entered : {ValidateBoxCondition(currTile)} {ValidateHorizontalLineCondition(currTile)} {ValidateVerticalLineCondition(currTile)}");


            if (ValidateBoxCondition(currTile) && ValidateHorizontalLineCondition(currTile) &&
                ValidateVerticalLineCondition(currTile))
            {
                Debug.Log("Condition True");
                return true;
            }

            Debug.Log("Condition False");
            //Current Tile Cannot have the Value the user provided, that is the reason we are reverting the value to Empty.
            currTile.TileVal = -1;
            return false;
        }

        public bool ValidateBoxCondition(SudokuTile tile)
        {
            SudokuTile[] allBoxTileArr = tile.currSudokuBox.GetAllBoxTiles();

            foreach (SudokuTile sudokuTile in allBoxTileArr)
            {
                if (sudokuTile == tile)
                    continue;

                if (tile.TileVal == sudokuTile.TileVal)
                {
                    return false;
                }
            }

            return true;
        }

        public bool ValidateHorizontalLineCondition(SudokuTile tile)
        {
            int rowNum = tile.currSudokuBox.sudokuBoxId.x * 3 + tile.sudokuTileId.x;
            //Debug.Log("Row Num : " + rowNum);

            SudokuTile[] tempTileArr = GetSudokuRow(rowNum);

            foreach (SudokuTile sudokuTile in tempTileArr)
            {
                if (sudokuTile == tile)
                    continue;

                if (sudokuTile.TileVal == tile.TileVal)
                {
                    return false;
                }
            }

            return true;
        }

        public bool ValidateVerticalLineCondition(SudokuTile tile)
        {
            int colNum = tile.currSudokuBox.sudokuBoxId.y * 3 + tile.sudokuTileId.y;
            //Debug.Log("Col Num : " + colNum);

            SudokuTile[] tempTileArr = GetSudokuColumn(colNum);

            foreach (SudokuTile sudokuTile in tempTileArr)
            {
                if (sudokuTile == tile)
                    continue;

                if (sudokuTile.TileVal == tile.TileVal)
                {
                    return false;
                }
            }

            return true;
        }
    }
}