using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YugantLoyaLibrary.SudokuSolver
{
    public class SudokuManager : MonoBehaviour
    {
        public static SudokuManager instance;

        public enum DifficultyLevelEnum
        {
            VeryEasy,
            Easy,
            Medium,
            Hard,
            Insane
        }

        [Serializable]
        public struct ValueDifficultyStruct
        {
            public DifficultyLevelEnum difficultyEnum;
            public int minValToFill;
            public int maxValToFill;
        }

        public DifficultyLevelEnum currDifficultyState;

        public static Action<SudokuTile> selectedTileEvent;

        public delegate void CheckConditionDelegate(SudokuTile tile, out bool conditionStatus);

        public static CheckConditionDelegate checkConditionEvent;

        public static SudokuTile currSudokuTile;
        private static SudokuTile _lastSudokuTile;

        public List<ValueDifficultyStruct> difficultyValueStructList;
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
            GenerateSudoku();
        }

        public void SetSudokuDifficulty(int difficultyVal)
        {
            currDifficultyState = (DifficultyLevelEnum)difficultyVal;
        }

        private int GetValueAccordingToDifficulty(DifficultyLevelEnum diffEnum)
        {
            foreach (ValueDifficultyStruct valueDifficultyStruct in difficultyValueStructList)
            {
                if (valueDifficultyStruct.difficultyEnum == diffEnum)
                {
                    return UnityEngine.Random.Range(valueDifficultyStruct.minValToFill,
                        valueDifficultyStruct.maxValToFill);
                }
            }

            return 0;
        }

        public void ResetSudoku()
        {
            for (int i = 0; i < 9; i++)
            {
                SudokuTile[] tiles = GetSudokuRow(i);

                foreach (SudokuTile tile in tiles)
                {
                    if (!tile.canBeChanged)
                        continue;

                    tile.TileVal = 0;
                }
            }
        }

        public void ClearCompleteSudoku()
        {
            for (int i = 0; i < 9; i++)
            {
                SudokuTile[] tiles = GetSudokuRow(i);

                foreach (SudokuTile tile in tiles)
                {
                    tile.DefaultFontColor();

                    if (!tile.canBeChanged)
                    {
                        tile.canBeChanged = true;
                    }

                    tile.TileVal = 0;
                }
            }
        }

        public void GenerateSudoku()
        {
            int showValues = GetValueAccordingToDifficulty(currDifficultyState);
            //Debug.Log("Show Val Count : " + showValues);

            ClearCompleteSudoku();

            int[][] puzzle = GetAllDataOfSudokuTiles();
            FillSudoku(showValues, puzzle);

            // for (var i = 0; i < finalGeneratedSudoku.Length; i++)
            // {
            //     int[] rows = finalGeneratedSudoku[i];
            //
            //     for (int j = 0; j < rows.Length; j++)
            //     {
            //         int val = rows[j];
            //         Debug.Log($"{i} {j}  :  {val}");
            //     }
            // }

            FillGeneratedSudokuValues(puzzle);
        }

        private void FillSudoku(int numberOfFilledCells, int[][] puzzle)
        {
            // Generate a complete Sudoku puzzle
            if (!FillSudoku(puzzle))
            {
                Debug.LogError("Failed to generate a complete Sudoku puzzle.");
            }

            // Remove cells to achieve the desired difficulty level
            RemoveCells(puzzle, 81 - numberOfFilledCells);
        }

        private bool FillSudoku(int[][] puzzle)
        {
            return SudokuSolver.instance.SolveFast(puzzle);
        }

        private void RemoveCells(int[][] puzzle, int cellsToRemove)
        {
            System.Random rand = new System.Random();

            for (int i = 0; i < cellsToRemove; i++)
            {
                int row = rand.Next(0, 9);
                int col = rand.Next(0, 9);

                // Ensure that the cell is not already empty
                while (puzzle[row][col] == 0)
                {
                    row = rand.Next(0, 9);
                    col = rand.Next(0, 9);
                }

                puzzle[row][col] = 0;
            }
        }

        public int[][] GetAllDataOfSudokuTiles()
        {
            int[] totalValArr = new int[81];
            int totalValindex = 0;

            for (int i = 0; i < 9; i++)
            {
                SudokuTile[] tiles = GetSudokuRow(i);

                foreach (SudokuTile tile in tiles)
                {
                    totalValArr[totalValindex] = tile.TileVal;
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
                    //Debug.Log($"Data [{i}][{j}] : {data[i][j]} ");
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

            //Update the NumberPad Keys
            UpdateNumberPadKeys();

            //Debug.Log("Curr Tile : " + currSudokuTile.gameObject.name);

            //Condition : Only when we start selecting tile for the first time, when lastSudoku Tile is Empty.
            if (_lastSudokuTile == null)
                return;

            //Debug.Log("Last Tile : " + _lastSudokuTile.gameObject.name);

            _lastSudokuTile.OnTileDeselect();
        }

        public void UpdateNumberPadKeys()
        {
            int[] usefulValueArr = GetUsefulKeyValue(currSudokuTile);
            //Debug.Log("Useful Val Arr Count : " + usefulValueArr.Length);
            NumberPadManager.instance.UpdateNumberKeys(usefulValueArr);
        }

        public int FindRowNumber(SudokuTile tile)
        {
            return tile.currSudokuBox.sudokuBoxId.x * 3 + tile.sudokuTileId.x;
        }

        public int FindColumnNumber(SudokuTile tile)
        {
            return tile.currSudokuBox.sudokuBoxId.y * 3 + tile.sudokuTileId.y;
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


        private void CheckCondition(SudokuTile currTile, out bool conditionStatus)
        {
            //Debug.Log($"Check Condition Entered !");

            // Debug.Log(
            //     $"Check Condition Entered : {ValidateBoxCondition(currTile)} {ValidateHorizontalLineCondition(currTile)} {ValidateVerticalLineCondition(currTile)}");


            if (ValidateBoxCondition(currTile) && ValidateHorizontalLineCondition(currTile) &&
                ValidateVerticalLineCondition(currTile))
            {
                //Debug.Log("Condition True");
                conditionStatus = true;
                return;
            }

            //Debug.Log("Condition False");
            conditionStatus = false;
        }

        private bool ValidateBoxCondition(SudokuTile tile)
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

        private bool ValidateHorizontalLineCondition(SudokuTile tile)
        {
            int rowNum = FindRowNumber(tile);
            //Debug.Log("Row Number : " + rowNum);

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

        private bool ValidateVerticalLineCondition(SudokuTile tile)
        {
            int colNum = FindColumnNumber(tile);
            //Debug.Log("Col Number : " + colNum);

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

        int[] GetMissingValuesInBox(SudokuTile tile)
        {
            List<int> missingValArr = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            SudokuTile[] allBoxTileArr = tile.currSudokuBox.GetAllBoxTiles();

            for (int index = allBoxTileArr.Length - 1; index >= 0; index--)
            {
                SudokuTile sudokuTile = allBoxTileArr[index];

                if (sudokuTile == tile)
                    continue;

                if (missingValArr.Contains(sudokuTile.TileVal))
                {
                    missingValArr.Remove(sudokuTile.TileVal);
                }
            }

            // foreach (int key in missingValArr)
            // {
            //     Debug.Log("Box Missing Values " + key);
            // }


            //Debug.Log("Box Missing Count : " + missingValArr.Count);
            return missingValArr.ToArray();
        }

        int[] GetMissingValuesInRow(SudokuTile tile)
        {
            List<int> missingValArr = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            int rowNum = FindRowNumber(tile);
            //Debug.Log("Row Number : " + rowNum);

            SudokuTile[] allHorizontalTileArr = GetSudokuRow(rowNum);

            for (int index = allHorizontalTileArr.Length - 1; index >= 0; index--)
            {
                SudokuTile sudokuTile = allHorizontalTileArr[index];

                if (sudokuTile == tile)
                    continue;

                if (missingValArr.Contains(sudokuTile.TileVal))
                {
                    missingValArr.Remove(sudokuTile.TileVal);
                }
            }

            // foreach (int key in missingValArr)
            // {
            //     Debug.Log("Horizontal Missing Keys " + key);
            // }

            //Debug.Log("Row Missing Count : " + missingValArr.Count);
            return missingValArr.ToArray();
        }

        int[] GetMissingValuesInColumn(SudokuTile tile)
        {
            List<int> missingValArr = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            int colNum = FindColumnNumber(tile);
            //Debug.Log("Col Number : " + rowNum);

            SudokuTile[] allVerticalTileArr = GetSudokuColumn(colNum);

            for (int index = allVerticalTileArr.Length - 1; index >= 0; index--)
            {
                SudokuTile sudokuTile = allVerticalTileArr[index];

                if (sudokuTile == tile)
                    continue;

                if (missingValArr.Contains(sudokuTile.TileVal))
                {
                    missingValArr.Remove(sudokuTile.TileVal);
                }
            }

            // foreach (int key in missingValArr)
            // {
            //     Debug.Log("Vertical Missing Val " + key);
            // }

            //Debug.Log("Col Missing Count : " + missingValArr.Count);
            return missingValArr.ToArray();
        }

        private int[] GetUsefulKeyValue(SudokuTile tile)
        {
            //Debug.Log("Tile : " + tile.gameObject.name, tile.gameObject);

            int[] sudokuBoxArr = GetMissingValuesInBox(tile);
            int[] sudokuHorizontalArr = GetMissingValuesInRow(tile);
            int[] sudokuVerticalArr = GetMissingValuesInColumn(tile);

            List<int> tempSetArr = sudokuBoxArr.Intersect(sudokuHorizontalArr).ToList();
            List<int> finalSetArr = tempSetArr.Intersect(sudokuVerticalArr).ToList();

            //Remember to add the value use for the Clear Button
            if (tile.canBeChanged)
            {
                finalSetArr.Add(0);
            }
            
            // foreach (int key in finalSetArr)
            // {
            //     Debug.Log("Useful Keys : " + key);
            // }

            return finalSetArr.ToArray();
        }

        public void FillGeneratedSudokuValues(int[][] generatedArr)
        {
            for (int i = 0; i < 9; i++)
            {
                SudokuTile[] rowTiles = GetSudokuRow(i);

                for (int index = 0; index < rowTiles.Length; index++)
                {
                    SudokuTile tile = rowTiles[index];
                    tile.TileVal = generatedArr[i][index];
                    tile.DefaultFontColor();
                    
                    if (tile.TileVal != 0)
                    {
                        tile.canBeChanged = false;
                    }
                }
            }
        }

        public void FillSudokuSolutionValues(int[][] solutionArr)
        {
            for (int i = 0; i < 9; i++)
            {
                SudokuTile[] rowTiles = GetSudokuRow(i);

                for (int index = 0; index < rowTiles.Length; index++)
                {
                    SudokuTile tile = rowTiles[index];

                    if (!tile.canBeChanged) continue;

                    tile.TileVal = solutionArr[i][index];
                    tile.SolutionFontColor();
                }
            }
        }

        public void FillSingleValue(int row, int col, int val)
        {
            SudokuTile[] rowTiles = GetSudokuRow(row);

            SudokuTile tile = rowTiles[col];

            if (!tile.canBeChanged)
                return;

            tile.TileVal = val;
            tile.SolutionFontColor();
        }
    }
}