using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace YugantLoyaLibrary.CrossWordGenerator
{
    public class CrossWordGenerator : MonoBehaviour
    {
        public class MainCrossWordBoxClass
        {
            public string word;
            public List<BoxTile> boxTileList;
        }

        public GameObject boxPrefab;
        public Transform boxContainerTrans;
        [SerializeField] private GridLayoutGroup boxContainerGridLayoutGroup;
        public int maxGridSize = 20;
        public Vector2Int startingGrid = new Vector2Int(7, 3);
        public List<string> wordList;
        public List<MainCrossWordBoxClass> mainCrossWordBoxList = new List<MainCrossWordBoxClass>();


        public List<BoxTile> totalBoxTiles = new List<BoxTile>();


        private void Start()
        {
            //CheckLargestWordLength();
            CreateGrid();
        }

        void CheckLargestWordLength()
        {
            int max = 0;
            foreach (string str in wordList)
            {
                if (str.Length > max)
                {
                    max = str.Length;
                }
            }

            Debug.Log("Largest Word Length : " + max);
            //maxGridSize = max;
            //boxContainerGridLayoutGroup.constraintCount = maxGridSize;
        }

        void ClearContainer()
        {
            for (int i = boxContainerTrans.childCount - 1; i >= 0; i--)
            {
                Destroy(boxContainerTrans.GetChild(i).gameObject);
            }

            totalBoxTiles.Clear();
            mainCrossWordBoxList.Clear();
        }

        [Button]
        void Generate()
        {
            if (wordList.Count < 2)
                return;

            List<string> words = PickRandomWord(2);

            if (words.Count < 2)
                return;

            List<char> commonLetterList = FindCommonLetters(words[0], words[1]);
            Debug.Log("Common Letter List Count : " + commonLetterList.Count);

            PlaceWords(words);
        }

        private void PlaceWords(List<string> words)
        {
            bool startingBoxUsed = false;

            foreach (string word in words)
            {
                // for (int i = 0; i < word.Length; i++)
                // {
                //     if (!startingBoxUsed)
                //     {
                //         startingBoxUsed = true;
                //        
                //     }
                //     
                // }

                int totalDirs = Enum.GetNames(typeof(BoxTile.WordFormationDir)).Length;
                int randomIndex = Random.Range(0, totalDirs);
                BoxTile.WordFormationDir dir = (BoxTile.WordFormationDir)randomIndex;


                bool isWordFormationPossible = IsWordFormationPossible(word, startingGrid, dir);


                //For Testing used the Break statement
                break;
            }
        }

        bool IsWordFormationPossible(string word, Vector2Int startingBox, BoxTile.WordFormationDir dir)
        {
            BoxTile boxTile = GetBoxByID(startingBox.x, startingBox.y);
            int letterIndex = 0;
            BoxTile currBoxTile = boxTile, tempBoxTile;


            switch (dir)
            {
                case BoxTile.WordFormationDir.Right:

                    while (letterIndex < word.Length)
                    {
                        currBoxTile.BoxData = word[letterIndex].ToString();
                        tempBoxTile = currBoxTile.GetNeighbour(BoxTile.BoxNeighbour.Left);
                        currBoxTile = tempBoxTile;
                        letterIndex++;
                    }


                    break;

                case BoxTile.WordFormationDir.Bottom:

                    while (letterIndex < word.Length)
                    {
                        currBoxTile.BoxData = word[letterIndex].ToString();
                        tempBoxTile = currBoxTile.GetNeighbour(BoxTile.BoxNeighbour.Bottom);
                        currBoxTile = tempBoxTile;
                        letterIndex++;
                    }

                    
                    break;
            }

            if (letterIndex != word.Length)
            {
                Debug.Log("Word Formation is Not Possible for : " + word);
                return false;
            }
            
            Debug.Log("Word Formation is Possible for : " + word);
            return true;
        }

        List<char> FindCommonLetters(string firstWord, string secondWord)
        {
            List<char> commonLetterList = new List<char>();

            foreach (char firstChar in firstWord)
            {
                foreach (char secondChar in secondWord)
                {
                    if (firstChar == secondChar)
                    {
                        commonLetterList.Add(firstChar);
                        break;
                    }
                }
            }

            return commonLetterList;
        }

        //[Button]
        void RefreshCrossWord()
        {
            ClearContainer();
            //CheckLargestWordLength();
            CreateGrid();
        }

        private void CreateGrid()
        {
            for (int i = 0; i < maxGridSize; i++)
            {
                for (int j = 0; j < maxGridSize; j++)
                {
                    GameObject box = Instantiate(boxPrefab, boxContainerTrans);
                    BoxTile boxTile = box.GetComponent<BoxTile>();
                    box.name = $"Box_{i}_{j}";
                    boxTile.BoxData = string.Empty;
                    boxTile.boxID = new Vector2Int(i, j);
                    totalBoxTiles.Add(boxTile);
                    //box.SetActive(false);
                }
            }

            SetBoxNeighbours();
        }

        void SetBoxNeighbours()
        {
            for (int i = 0; i < totalBoxTiles.Count; i++)
            {
                BoxTile currBox = totalBoxTiles[i];
                for (int j = 0; j < Enum.GetNames(typeof(BoxTile.BoxNeighbour)).Length; j++)
                {
                    BoxTile.BoxNeighbour side = (BoxTile.BoxNeighbour)j;
                    UpdateNeighbourBox(currBox, side);
                }
            }
        }

        private void UpdateNeighbourBox(BoxTile currBox, BoxTile.BoxNeighbour neighbour)
        {
            switch (neighbour)
            {
                case BoxTile.BoxNeighbour.Left:

                    if (currBox.boxID.y - 1 >= 0)
                    {
                        currBox.leftBox = GetBoxByID(currBox.boxID.x, currBox.boxID.y - 1);
                    }

                    break;

                case BoxTile.BoxNeighbour.Right:

                    if (currBox.boxID.y + 1 < maxGridSize)
                    {
                        currBox.rightBox = GetBoxByID(currBox.boxID.x, currBox.boxID.y + 1);
                    }

                    break;

                case BoxTile.BoxNeighbour.Top:

                    if (currBox.boxID.x - 1 >= 0)
                    {
                        currBox.topBox = GetBoxByID(currBox.boxID.x - 1, currBox.boxID.y);
                    }

                    break;

                case BoxTile.BoxNeighbour.Bottom:

                    if (currBox.boxID.x + 1 < maxGridSize)
                    {
                        currBox.bottomBox = GetBoxByID(currBox.boxID.x + 1, currBox.boxID.y);
                    }

                    break;
            }
        }

        List<string> PickRandomWord(int totalWordsToPick)
        {
            List<string> words = new List<string>(wordList.Count);
            List<int> usedIndexList = new List<int>();

            if (totalWordsToPick > wordList.Count)
                return null;

            while (words.Count < totalWordsToPick)
            {
                int randomIndex = Random.Range(0, wordList.Count);

                if (!usedIndexList.Contains(randomIndex))
                {
                    words.Add(wordList[randomIndex]);
                    usedIndexList.Add(randomIndex);
                    Debug.Log($"Random Word Selected {words.Count} : {wordList[randomIndex]}");
                }
            }


            return words;
        }

        private BoxTile GetBoxByID(int i, int j)
        {
            int targetIndex = i * maxGridSize + j;

            for (int k = 0; k < totalBoxTiles.Count; k++)
            {
                if (k == targetIndex)
                {
                    return totalBoxTiles[k];
                }
            }

            return null;
        }
    }
}