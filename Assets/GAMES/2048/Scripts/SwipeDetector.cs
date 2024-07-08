using UnityEngine;

namespace YugantLoyaLibrary.Game2048
{
    namespace SwipeHandler
    {
        public class SwipeDetector : MonoBehaviour
        {
            void OnEnable()
            {
                SwipeHandler.OnSwipe += HandleSwipe;
            }

            void OnDisable()
            {
                SwipeHandler.OnSwipe -= HandleSwipe;
            }

            void HandleSwipe(SwipeHandler.Direction direction)
            {
                if (GameManager.CurrGameState != GameState.WaitingInput)
                    return;
                
                switch (direction)
                {
                    case SwipeHandler.Direction.Right:
                        MoveRight();
                        break;
                    case SwipeHandler.Direction.Left:
                        MoveLeft();
                        break;
                    case SwipeHandler.Direction.Up:
                        MoveUp();
                        break;
                    case SwipeHandler.Direction.Down:
                        MoveDown();
                        break;
                }
            }

            void MoveRight()
            {
                // Your code to handle swipe right
                //Debug.Log("Right!");
                GameManager.Instance.ShiftBlocks(Vector2.right);
            }

            void MoveLeft()
            {
                // Your code to handle swipe left
                //Debug.Log("Left!");
                GameManager.Instance.ShiftBlocks(Vector2.left);
            }

            void MoveUp()
            {
                // Your code to handle swipe up
                //Debug.Log("Up!");
                GameManager.Instance.ShiftBlocks(Vector2.up);
            }

            void MoveDown()
            {
                // Your code to handle swipe down
                //Debug.Log("Down!");
                GameManager.Instance.ShiftBlocks(Vector2.down);
            }
        }
    }
}