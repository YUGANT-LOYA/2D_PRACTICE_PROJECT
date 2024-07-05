using System;
using System.Collections.Generic;
using System.Linq;
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
        private Camera _cam;
        [SerializeField] private int width = 4, height = 4;
        [SerializeField] private Node nodePrefab;
        [SerializeField] private Block blockPrefab;
        [SerializeField] private SpriteRenderer boardSpriteRenderer;
        private List<Node> _nodesList;
        private List<Block> _blocksList;
        [SerializeField] private List<BlockType> blockTypeList;
        [SerializeField] private GameState _currGameState;
        private int currRound = 0;

        BlockType GetBlockTypeByValue(int val) => blockTypeList.First(type => type.value == val);

        private void ChangeState(GameState newState)
        {
            _currGameState = newState;

            switch (_currGameState)
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
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Awake()
        {
            _cam = Camera.main;
        }

        private void Start()
        {
            GenerateGrid();
        }

        private void Update()
        {
            if (_currGameState != GameState.WaitingInput)
                return;

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ShiftBlocks(Vector2.left);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ShiftBlocks(Vector2.right);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ShiftBlocks(Vector2.up);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ShiftBlocks(Vector2.down);
            }
        }

        private void ShiftBlocks(Vector2 dir)
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
                Node nextNode = block.currBlockNode;

                do
                {
                    block.SetBlock(nextNode);
                    Node possibleNode = GetNodeAtPosition(nextNode.Pos + dir);

                    if (possibleNode != null)
                    {
                        if (possibleNode.occupiedBlock == null)
                        {
                            nextNode = possibleNode;
                        }
                    }
                } while (nextNode != block.currBlockNode);

                block.transform.position = block.currBlockNode.Pos;
            }

            ChangeState(GameState.WaitingInput);
        }

        Node GetNodeAtPosition(Vector2 pos)
        {
            return _nodesList.FirstOrDefault(temp => temp.Pos == pos);
        }

        void GenerateGrid()
        {
            _currGameState = GameState.GenerateLevel;
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
                _nodesList.Where(n => n.occupiedBlock == null).OrderBy(b => Random.value);

            foreach (Node node in freeNodes.Take(amount))
            {
                Block block = Instantiate(blockPrefab, node.Pos, Quaternion.identity);
                block.Init(GetBlockTypeByValue(Random.value >= 0.75f ? 4 : 2));
                block.SetBlock(node);
                _blocksList.Add(block);
            }

            if (freeNodes.Count() == 1)
            {
                //Game Lost
                return;
            }

            ChangeState(GameState.WaitingInput);
        }
    }
}