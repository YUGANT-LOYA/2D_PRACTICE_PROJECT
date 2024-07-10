using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

namespace YugantLoyaLibrary.Game_2048
{
    public class BlockBase : MonoBehaviour
    {
        public int Value { get; set; }
        public GridBase CurrGrid { get; set; }
        public bool IsMergeAllowed { get; set; }
        public BlockBase MergeBlockWith { get; set; }

        public Vector3 Pos
        {
            get => transform.position;
            set => transform.position = value;
        }

        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] private TextMeshPro blockText;
        [SerializeField] MMF_Player mmPlayerFeedbacks;

        private Dictionary<Type, MMF_Feedback> feedbackDictionary = new Dictionary<Type, MMF_Feedback>();


        private void OnEnable()
        {
            GameManager.ChangeGameStateEvent += GameStateChanged;
        }

        private void OnDisable()
        {
            GameManager.ChangeGameStateEvent -= GameStateChanged;
        }

        private void GameStateChanged(GameState fromState, GameState toState)
        {
            
        }

        public void Init(BlockType type)
        {
            Value = type.value;
            spriteRenderer.color = type.color;
            blockText.text = Value.ToString();
        }

        public void SetBlock(GridBase newNode)
        {
            if (CurrGrid != null)
            {
                CurrGrid.OccupiedBlockBase = null;
            }

            CurrGrid = newNode;
            CurrGrid.OccupiedBlockBase = this;
        }

        public void MergeBlock(BlockBase blockToMergeWith)
        {
            MergeBlockWith = blockToMergeWith;
            CurrGrid.OccupiedBlockBase = null;
            MergeBlockWith.IsMergeAllowed = true;
        }

        public bool CanMerge(int val) => Value == val && !IsMergeAllowed && MergeBlockWith == null;
    }
}