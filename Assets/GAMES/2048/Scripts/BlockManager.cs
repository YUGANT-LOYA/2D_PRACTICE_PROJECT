using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using YugantLoyaLibrary.Game2048;
using YugantLoyaLibrary.Game2048.SwipeHandler;
using Random = Unity.Mathematics.Random;

namespace YugantLoyaLibrary.Game_2048
{
    public class BlockManager : MonoBehaviour
    {
        private Camera _cam;
        [SerializeField] private int width = 4, height = 4;
        public int currRound = 0;
        [SerializeField] private GridBase nodePrefab;
        [SerializeField] private BlockBase blockPrefab;
        [SerializeField] private SpriteRenderer boardSpriteRenderer;
        [SerializeField] private float blockMovingTime = 0.4f, floatingTxtTime = 1f, overShootMovement = 1.2f;
        [SerializeField] private Ease blockMovingEase = Ease.OutBack;
        private List<GridBase> _nodesList;
        private List<BlockBase> _blocksList;
        [SerializeField] private List<BlockType> blockTypeList;

        private void OnEnable()
        {
            SwipeHandler.OnSwipe += Swipe;
        }


        private void OnDisable()
        {
            SwipeHandler.OnSwipe -= Swipe;
        }

        private void Swipe(SwipeHandler.Direction direction)
        {
            switch (direction)
            {
                case SwipeHandler.Direction.Left:
                    ShiftBlocks(Vector2.left);
                    break;
                case SwipeHandler.Direction.Right:
                    ShiftBlocks(Vector2.right);
                    break;
                case SwipeHandler.Direction.Up:
                    ShiftBlocks(Vector2.up);
                    break;
                case SwipeHandler.Direction.Down:
                    ShiftBlocks(Vector2.down);
                    break;
            }
        }

        private void Awake()
        {
            _cam = Camera.main;
        }

        BlockType GetBlockTypeByValue(int val) => blockTypeList.First(type => type.value == val);

        public void GenerateGrid()
        {
            currRound = 0;
            _nodesList = new List<GridBase>();
            _blocksList = new List<BlockBase>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GridBase node = Instantiate(nodePrefab, new Vector2(x, y), Quaternion.identity);
                    _nodesList.Add(node);
                }
            }

            Vector2 centre = new Vector2(width / 2f - 0.5f, height / 2f - 0.5f);

            SpriteRenderer board = Instantiate(boardSpriteRenderer, centre, Quaternion.identity);
            board.size = new Vector2(width, height);
            _cam.transform.position = new Vector3(centre.x, centre.y, -10f);

            Debug.Log("Generate Grid Completed !");
            GameManager.ChangeGameStateEvent?.Invoke(GameManager.CurrGameState, GameState.SpawningBlocks);
        }

        public void SpawnBlock(GridBase node, int val)
        {
            //Debug.Log("Block Spawning");
            BlockBase block = Instantiate(blockPrefab, node.Pos, Quaternion.identity);
            block.Init(GetBlockTypeByValue(val));
            block.SetBlock(node);
            _blocksList.Add(block);
        }

        public void ShiftBlocks(Vector2 dir)
        {
            GameManager.ChangeGameStateEvent?.Invoke(GameManager.CurrGameState, GameState.Moving);

            List<BlockBase> orderBlockList =
                _blocksList.OrderBy(temp => temp.Pos.x).ThenBy(temp => temp.Pos.y).ToList();

            if (dir == Vector2.right || dir == Vector2.up)
            {
                orderBlockList.Reverse();
            }

            foreach (BlockBase block in orderBlockList)
            {
                GridBase nextNode = block.CurrGrid;

                do
                {
                    block.SetBlock(nextNode);
                    GridBase possibleNode = GetNodeAtPosition((Vector2)nextNode.Pos + dir);

                    if (possibleNode != null)
                    {
                        if (possibleNode.OccupiedBlockBase != null &&
                            possibleNode.OccupiedBlockBase.CanMerge(block.Value))
                        {
                            block.MergeBlock(possibleNode.OccupiedBlockBase);
                        }
                        else if (possibleNode.OccupiedBlockBase == null)
                        {
                            nextNode = possibleNode;
                        }
                    }
                } while (nextNode != block.CurrGrid);


                //block.PlayMoveEffect(block.currBlockNode);
            }

            Sequence seq = DOTween.Sequence();

            foreach (BlockBase block in orderBlockList)
            {
                Vector3 movePoint = block.MergeBlockWith != null
                    ? block.MergeBlockWith.CurrGrid.Pos
                    : block.CurrGrid.Pos;
                seq.Insert(0,
                    block.transform.DOMove(movePoint, blockMovingTime).SetEase(blockMovingEase, overShootMovement));
            }

            seq.OnComplete(() =>
            {
                foreach (BlockBase block in orderBlockList.Where(temp => temp.MergeBlockWith != null))
                {
                    MergeBlock(block.MergeBlockWith, block);

                    TextMeshPro floatingTxt = ObjectPoolSystem.Instance
                        .GetObjectByType(PoolObjectType.FloatingText).GetComponent<TextMeshPro>();

                    floatingTxt.text = (block.Value * 2).ToString();

                    if (floatingTxt.gameObject != null)
                    {
                        floatingTxt.transform.position = block.Pos;
                        floatingTxt.transform.DOMoveY(floatingTxt.transform.position.y + 1.5f, floatingTxtTime);
                        Color txtColor = floatingTxt.color;

                        floatingTxt.DOFade(0f, floatingTxtTime).OnComplete(() =>
                        {
                            floatingTxt.color = txtColor;
                            ObjectPoolSystem.Instance.ReturnToPool(PoolObjectType.FloatingText,
                                floatingTxt.gameObject);
                        });
                    }
                }

                GameManager.ChangeGameStateEvent?.Invoke(GameManager.CurrGameState, GameState.SpawningBlocks);
            });

            //ChangeState(GameState.WaitingInput); 
        }

        void MergeBlock(BlockBase baseBlock, BlockBase mergingBlock)
        {
            SpawnBlock(baseBlock.CurrGrid, baseBlock.Value * 2);
            RemoveBlock(baseBlock);
            RemoveBlock(mergingBlock);
        }

        void RemoveBlock(BlockBase block)
        {
            _blocksList.Remove(block);
            Destroy(block.gameObject);
        }

        GridBase GetNodeAtPosition(Vector2 pos)
        {
            return _nodesList.FirstOrDefault(temp => (Vector2)temp.Pos == pos);
        }

        public void SpawningBlocks(int amount)
        {
            IOrderedEnumerable<GridBase> freeNodes =
                _nodesList.Where(n => n.OccupiedBlockBase == null).OrderBy(b => UnityEngine.Random.value);

            foreach (GridBase node in freeNodes.Take(amount))
            {
                SpawnBlock(node, UnityEngine.Random.value >= 0.75f ? 4 : 2);
            }

            if (!freeNodes.Any())
            {
                //Game Lost
                return;
            }

            GameManager.ChangeGameStateEvent?.Invoke(GameManager.CurrGameState, GameState.WaitingInput);
        }
    }
}