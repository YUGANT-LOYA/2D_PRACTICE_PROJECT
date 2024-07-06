using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

namespace YugantLoyaLibrary.Game2048
{
    public class Block : MonoBehaviour
    {
        public int value;
        public Node currBlockNode;
        public bool isMerging;
        public Vector2 Pos => transform.position;
        public Block mergingBlock;
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
            value = type.value;
            spriteRenderer.color = type.color;
            blockText.text = value.ToString();
        }

        public void SetBlock(Node newNode)
        {
            if (currBlockNode != null)
            {
                currBlockNode.occupiedBlock = null;
            }

            currBlockNode = newNode;
            currBlockNode.occupiedBlock = this;
        }

        public void MergeBlock(Block blockToMergeWith)
        {
            mergingBlock = blockToMergeWith;
            currBlockNode.occupiedBlock = null;
            mergingBlock.isMerging = true;
        }

        public bool CanMerge(int val) => value == val && !isMerging && mergingBlock == null;
        
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
    }
}