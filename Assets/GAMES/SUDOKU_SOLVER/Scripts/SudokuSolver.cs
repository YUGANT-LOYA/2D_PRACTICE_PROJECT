using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YugantLoyaLibrary.SudokuSolver
{
    public class SudokuSolver : MonoBehaviour
    {
        public static SudokuSolver instance;
        
        [SerializeField] private float waitTimeForEachGrid = 0.02f;
        public bool isSlowSolutionCancelled;

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
        
        public void CancelSlowSolution()
        {
            isSlowSolutionCancelled = true;
            
        }
        
        static void ShuffleArray<T>(T[] array)
        {
            System.Random rand = new System.Random();
            
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = rand.Next(0, i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }
        
        void ShuffleList<T>(List<T> list)
        {
            System.Random rand = new System.Random();
            
            int n = list.Count;
            
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
        
        public void FillSudokuFast()
        {
            if (!Application.isPlaying)
            {
                Debug.LogError("Only Runs when Game is in Playing State !");
                return;
            }

            int[][] puzzle = SudokuManager.instance.GetAllDataOfSudokuTiles();

            if (puzzle.Length != 9 || puzzle[0].Length != 9)
            {
                Debug.LogError("Invalid puzzle size. Expected a 9x9 grid.");
            }

            int[][] solution = new int[9][];
            
            for (int i = 0; i < 9; i++)
            {
                solution[i] = new int[9];
                for (int j = 0; j < 9; j++)
                {
                    solution[i][j] = puzzle[i][j];
                }
            }

            if (SolveFast(solution))
            {
                Debug.Log("Answer Found !");
                SudokuManager.instance.FillSudokuSolutionValues(solution);
            }
            else
            {
                Debug.LogError("No solution exists for the given puzzle.");
            }
        }

        public bool SolveFast(int[][] grid)
        {
            int row, col;
            if (!FindEmptyLocation(grid, out row, out col))
            {
                return true; // Puzzle solved
            }
            
            int[] numArr = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            ShuffleArray(numArr);
            
            for (int index = 0; index < numArr.Length; index++)
            {
                int num = numArr[index];
                
                if (IsSafe(grid, row, col, num))
                {
                    grid[row][col] = num;

                    if (SolveFast(grid))
                    {
                        SudokuManager.instance.FillSingleValue(row, col, num);
                        return true;
                    }

                    grid[row][col] = 0; // Undo the assignment and try again

                    SudokuManager.instance.FillSingleValue(row, col, num);
                }
            }

            return false; // Backtrack
        }
        
        public void FillSudokuSlow()
        {
            StartCoroutine(FillSlow());
        }

        private IEnumerator FillSlow()
        {
            if (!Application.isPlaying)
            {
                Debug.LogError("Only Runs when Game is in Playing State !");
                yield break;
            }

            SudokuUI.instance.SlowSolutionRunning();
            
            int[][] puzzle = SudokuManager.instance.GetAllDataOfSudokuTiles();

            if (puzzle.Length != 9 || puzzle[0].Length != 9)
            {
                Debug.LogError("Invalid puzzle size. Expected a 9x9 grid.");
                yield break;
            }

            int[][] solution = new int[9][];
            for (int i = 0; i < 9; i++)
            {
                solution[i] = new int[9];
                for (int j = 0; j < 9; j++)
                {
                    solution[i][j] = puzzle[i][j];
                }
            }

            using (IEnumerator<bool> solver = SolveSlow(solution).GetEnumerator())
            {
                while (solver.MoveNext() && !isSlowSolutionCancelled)
                {
                    if (solver.Current)
                    {
                        //Debug.Log("Answer Found !");
                        SudokuManager.instance.FillSudokuSolutionValues(solution);
                        isSlowSolutionCancelled = false;
                        SudokuUI.instance.ResetAllButtons();
                        yield break;
                    }

                    yield return new WaitForSeconds(waitTimeForEachGrid);
                }

                if (isSlowSolutionCancelled)
                {
                    isSlowSolutionCancelled = false;
                    SudokuUI.instance.ResetAllButtons();
                    SudokuManager.instance.FillSudokuSolutionValues(puzzle);
                    yield break;
                }
            }

            Debug.LogError("No solution exists for the given puzzle.");
        }

        private IEnumerable<bool> SolveSlow(int[][] grid)
        {
            int row, col;
            if (!FindEmptyLocation(grid, out row, out col))
            {
                yield return true; // Puzzle solved
                yield break;
            }

            int[] numArr = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            ShuffleArray(numArr);
            
            for (int index = 0; index < numArr.Length; index++)
            {
                int num = numArr[index];
                
                if (IsSafe(grid, row, col, num))
                {
                    grid[row][col] = num;

                    foreach (var result in SolveSlow(grid))
                    {
                        yield return result;
                    }

                    grid[row][col] = 0; // Undo the assignment and try again
                    SudokuManager.instance.FillSudokuSolutionValues(grid);
                    
                    yield return false; // Introduce a delay
                }
            }

            yield return false; // Backtrack
        }


        private static bool FindEmptyLocation(int[][] grid, out int row, out int col)
        {
            for (row = 0; row < 9; row++)
            {
                for (col = 0; col < 9; col++)
                {
                    if (grid[row][col] == 0)
                    {
                        return true;
                    }
                }
            }

            row = -1;
            col = -1;
            return false;
        }

        private static bool UsedInRow(int[][] grid, int row, int num)
        {
            for (int col = 0; col < 9; col++)
            {
                if (grid[row][col] == num)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool UsedInColumn(int[][] grid, int col, int num)
        {
            for (int row = 0; row < 9; row++)
            {
                if (grid[row][col] == num)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool UsedInBox(int[][] grid, int boxStartRow, int boxStartCol, int num)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (grid[row + boxStartRow][col + boxStartCol] == num)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool IsSafe(int[][] grid, int row, int col, int num)
        {
            return !UsedInRow(grid, row, num) &&
                   !UsedInColumn(grid, col, num) &&
                   !UsedInBox(grid, row - row % 3, col - col % 3, num);
        }
    }
}