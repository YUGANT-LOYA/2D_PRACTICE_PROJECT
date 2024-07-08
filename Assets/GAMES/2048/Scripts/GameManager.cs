using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace YugantLoyaLibrary.Game2048
{
    [Serializable]
    public struct BlockType
    {
        public int value;
        public Color color;
    }

    public enum GameState
    {
        GenerateLevel,
        SpawningBlocks,
        WaitingInput,
        Moving,
        Win,
        Lose
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        private Camera _cam;
        [SerializeField] private int width = 4, height = 4;
        [SerializeField] private Node nodePrefab;
        [SerializeField] private Block blockPrefab;
        [SerializeField] private SpriteRenderer boardSpriteRenderer;
        [SerializeField] private float blockMovingTime = 0.4f, floatingTxtTime = 1f, overShootMovement = 1.2f;
        [SerializeField] private Ease blockMovingEase = Ease.InOutQuad;
        private List<Node> _nodesList;
        private List<Block> _blocksList;
        [SerializeField] private List<BlockType> blockTypeList;
        public static GameState CurrGameState { get; private set; }
        private int currRound = 0;

        BlockType GetBlockTypeByValue(int val) => blockTypeList.First(type => type.value == val);

        public void ChangeState(GameState newState)
        {
            CurrGameState = newState;

            switch (CurrGameState)
            {
                case GameState.GenerateLevel:
                    GenerateGrid();
                    break;
                case GameState.SpawningBlocks:
                    SpawningBlocks(currRound++ == 0 ? 2 : 1);
                    break;
                case GameState.WaitingInput:
                    break;
                case GameState.Moving:
                    break;
                case GameState.Win:
                    break;
                case GameState.Lose:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            Application.targetFrameRate = 60;
            _cam = Camera.main;
        }

        private void Start()
        {
            GenerateGrid();
        }

        private void Update()
        {
            if (CurrGameState != GameState.WaitingInput)
                return;

            // if (Input.GetKeyDown(KeyCode.LeftArrow))
            // {
            //     ShiftBlocks(Vector2.left);
            // }
            //
            // if (Input.GetKeyDown(KeyCode.RightArrow))
            // {
            //     ShiftBlocks(Vector2.right);
            // }
            //
            // if (Input.GetKeyDown(KeyCode.UpArrow))
            // {
            //     ShiftBlocks(Vector2.up);
            // }
            //
            // if (Input.GetKeyDown(KeyCode.DownArrow))
            // {
            //     ShiftBlocks(Vector2.down);
            // }
        }

        void SpawnBlock(Node node, int val)
        {
            //Debug.Log("Block Spawning");
            Block block = Instantiate(blockPrefab, node.Pos, Quaternion.identity);
            block.Init(GetBlockTypeByValue(val));
            block.SetBlock(node);
            _blocksList.Add(block);
        }

        public void ShiftBlocks(Vector2 dir)
        {
            ChangeState(GameState.Moving);

            List<Block> orderBlockList =
                _blocksList.OrderBy(temp => temp.Pos.x).ThenBy(temp => temp.Pos.y).ToList();

            if (dir == Vector2.right || dir == Vector2.up)
            {
                orderBlockList.Reverse();
            }

            foreach (Block block in orderBlockList)
            {
                Node nextNode = block.CurrNode;

                do
                {
                    block.SetBlock(nextNode);
                    Node possibleNode = GetNodeAtPosition((Vector2)nextNode.Pos + dir);

                    if (possibleNode != null)
                    {
                        if (possibleNode.OccupiedNumberBlock != null && possibleNode.OccupiedNumberBlock.CanMerge(block.Value))
                        {
                            block.MergeBlock(possibleNode.OccupiedNumberBlock);
                        }
                        else if (possibleNode.OccupiedNumberBlock == null)
                        {
                            nextNode = possibleNode;
                        }
                    }
                } while (nextNode != block.CurrNode);


                //block.PlayMoveEffect(block.currBlockNode);
            }

            Sequence seq = DOTween.Sequence();

            foreach (Block block in orderBlockList)
            {
                Vector3 movePoint = block.MergeBlockWith != null
                    ? block.MergeBlockWith.CurrNode.Pos
                    : block.CurrNode.Pos;
                seq.Insert(0,
                    block.transform.DOMove(movePoint, blockMovingTime).SetEase(blockMovingEase, overShootMovement));
            }

            seq.OnComplete(() =>
            {
                foreach (Block block in orderBlockList.Where(temp => temp.MergeBlockWith != null))
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

                ChangeState(GameState.SpawningBlocks);
            });

            //ChangeState(GameState.WaitingInput); 
        }

        void MergeBlock(Block baseBlock, Block mergingBlock)
        {
            SpawnBlock(baseBlock.CurrNode, baseBlock.Value * 2);
            RemoveBlock(baseBlock);
            RemoveBlock(mergingBlock);
        }

        void RemoveBlock(Block block)
        {
            _blocksList.Remove(block);
            Destroy(block.gameObject);
        }

        Node GetNodeAtPosition(Vector2 pos)
        {
            return _nodesList.FirstOrDefault(temp => (Vector2)temp.Pos == pos);
        }

        void GenerateGrid()
        {
            CurrGameState = GameState.GenerateLevel;
            currRound = 0;
            _nodesList = new List<Node>();
            _blocksList = new List<Block>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Node node = Instantiate(nodePrefab, new Vector2(x, y), Quaternion.identity);
                    _nodesList.Add(node);
                }
            }

            Vector2 centre = new Vector2(width / 2f - 0.5f, height / 2f - 0.5f);

            SpriteRenderer board = Instantiate(boardSpriteRenderer, centre, Quaternion.identity);
            board.size = new Vector2(width, height);
            _cam.transform.position = new Vector3(centre.x, centre.y, -10f);

            ChangeState(GameState.SpawningBlocks);
        }

        void SpawningBlocks(int amount)
        {
            IOrderedEnumerable<Node> freeNodes =
                _nodesList.Where(n => n.OccupiedNumberBlock == null).OrderBy(b => Random.value);

            foreach (Node node in freeNodes.Take(amount))
            {
                SpawnBlock(node, Random.value >= 0.75f ? 4 : 2);
            }

            if (!freeNodes.Any())
            {
                //Game Lost
                return;
            }

            ChangeState(GameState.WaitingInput);
        }
    }
}