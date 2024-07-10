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
            }

            void MoveLeft()
            {
                // Your code to handle swipe left
                //Debug.Log("Left!");
            }

            void MoveUp()
            {
                // Your code to handle swipe up
                //Debug.Log("Up!");
            }

            void MoveDown()
            {
                // Your code to handle swipe down
                //Debug.Log("Down!");
            }
        }
    }
}