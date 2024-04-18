using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace YugantLoyaLibrary.CrossWordGenerator
{
    public class BoxTile : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI boxText;
        public Vector2Int boxID;
        public BoxTile leftBox, rightBox, topBox, bottomBox;

        public string BoxData
        {
            get => boxText.text;
            set => boxText.text = value;
        }

        public enum BoxNeighbour
        {
            Left,
            Right,
            Top,
            Bottom
        }

        public enum WordFormationDir
        {
            Right,
            Bottom
        }

        public WordFormationDir wordFormationDir;
        public bool isBoxUsed, isWordStartingLetter;


        public BoxTile GetNeighbour(BoxNeighbour side)
        {
            switch (side)
            {
                case BoxNeighbour.Left:

                    if (leftBox != null)
                    {
                        return leftBox;
                    }

                    break;

                case BoxNeighbour.Right:

                    if (rightBox != null)
                    {
                        return rightBox;
                    }

                    break;

                case BoxNeighbour.Top:

                    if (topBox != null)
                    {
                        return topBox;
                    }

                    break;

                case BoxNeighbour.Bottom:

                    if (bottomBox != null)
                    {
                        return bottomBox;
                    }

                    break;
            }

            return null;
        }
    }
}