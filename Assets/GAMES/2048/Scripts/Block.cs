using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

namespace YugantLoyaLibrary.Game2048
{
    public class Block : BlockBase
    {
        public int Value { get; set; }
        public bool IsMergeAllowed { get; set; }
        public Node CurrNode { get; set; }
        public Block MergeBlockWith { get; set; }
        
        public Vector3 Pos
        {
            get => transform.position;
            set => transform.position = value;
        }

        public void SetBlock(Node node)
        {
            if (CurrNode != null)
            {
                CurrNode.OccupiedNumberBlock = null;
            }

            CurrNode = node;
            CurrNode.OccupiedNumberBlock = this;
        }
        
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] private TextMeshPro blockText;
        [SerializeField] MMF_Player mmPlayerFeedbacks;

        private Dictionary<Type, MMF_Feedback> feedbackDictionary = new Dictionary<Type, MMF_Feedback>();

        private void Awake()
        {
            if (mmPlayerFeedbacks != null)
            {
                foreach (var feedback in mmPlayerFeedbacks.FeedbacksList)
                {
                    Type feedbackType = feedback.GetType();

                    if (!feedbackDictionary.TryAdd(feedbackType, feedback))
                    {
                        Debug.LogWarning(
                            $"Duplicate feedback type found: {feedbackType.Name}. Ensure each feedback type is unique.");
                    }
                }
            }
        }

        public void Init(BlockType type)
        {
            Value = type.value;
            spriteRenderer.color = type.color;
            blockText.text = Value.ToString();
        }
        
        // public void SetBlock(Node newNode)
        // {
        //     if (CurrNode != null)
        //     {
        //         CurrNode.OccupiedNumberBlock = null;
        //     }
        //
        //     CurrNode = newNode;
        //     CurrNode.OccupiedNumberBlock = this;
        // }

        // public void MergeBlock(Block blockToMergeWith)
        // {
        //     mergingBlock = blockToMergeWith;
        //     CurrNode.OccupiedNumberBlock = null;
        //     mergingBlock.IsMergeAllowed = true;
        // }

        public bool CanMerge(int val) => Value == val && !IsMergeAllowed && MergeBlockWith == null;

        public void PlayMoveEffect(Node targetNode)
        {
            MMF_Position positionFeedBack = GetFeedback<MMF_Position>();
            positionFeedBack.InitialPosition = transform.position;
            positionFeedBack.DestinationPosition = targetNode.transform.position;
            mmPlayerFeedbacks?.PlayFeedbacks();
        }

        public T GetFeedback<T>() where T : MMF_Feedback
        {
            Type feedbackType = typeof(T);

            if (feedbackDictionary.TryGetValue(feedbackType, out MMF_Feedback feedback))
            {
                if (feedback is T typedFeedback)
                {
                    return typedFeedback;
                }
            }
            else
            {
                Debug.LogWarning($"{feedbackType.Name} feedback is not assigned or not found in MMF_Player.");
            }

            return null;
        }

        public void MergeBlock(Block mergeBlockWith)
        {
            MergeBlockWith = mergeBlockWith;
            CurrNode.OccupiedNumberBlock = null;
            MergeBlockWith.IsMergeAllowed = true;
        }
    }
}