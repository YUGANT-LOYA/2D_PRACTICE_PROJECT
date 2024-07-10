using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace YugantLoyaLibrary.Game_2048
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

        public static Action<GameState, GameState> ChangeGameStateEvent;

        [SerializeField] private BlockManager blockManager;

        public static GameState CurrGameState { get; private set; }

        private void OnEnable()
        {
            ChangeGameStateEvent += GameStateChanged;
        }

        private void OnDisable()
        {
            ChangeGameStateEvent -= GameStateChanged;
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
        }

        private void Start()
        {
            ChangeGameStateEvent?.Invoke(CurrGameState, GameState.GenerateLevel);
        }

        void GameStateChanged(GameState fromState, GameState toState)
        {
            CurrGameState = toState;

            switch (CurrGameState)
            {
                case GameState.GenerateLevel:
                    blockManager.GenerateGrid();
                    break;
                case GameState.SpawningBlocks:
                    blockManager.SpawningBlocks(blockManager.currRound++ == 0 ? 2 : 1);
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
                    throw new ArgumentOutOfRangeException(nameof(toState), toState, null);
            }
        }


        private void Update()
        {
            if (CurrGameState != GameState.WaitingInput)
                return;

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                blockManager.ShiftBlocks(Vector2.left);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                blockManager.ShiftBlocks(Vector2.right);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                blockManager.ShiftBlocks(Vector2.up);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                blockManager.ShiftBlocks(Vector2.down);
            }
        }
    }
}