using UnityEngine;

namespace YugantLoyaLibrary.Game2048
{
    namespace SwipeHandler
    {
        public class SwipeHandler : MonoBehaviour
        {
            public enum Direction
            {
                None,
                Left,
                Right,
                Up,
                Down
            }

            public delegate void SwipeAction(Direction direction);

            public static event SwipeAction OnSwipe;

            private Vector2 startTouchPosition;
            private Vector2 endTouchPosition;
            public bool isSwiping = false;
            [SerializeField] float swipeThreshold = 50f;

            void Update()
            {
                if (Input.GetMouseButtonDown(0))
                {
                    startTouchPosition = Input.mousePosition;
                    isSwiping = true;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    endTouchPosition = Input.mousePosition;
                    DetectSwipe();
                    isSwiping = false;
                }
            }

            void DetectSwipe()
            {
                float swipeDistance = Vector2.Distance(startTouchPosition, endTouchPosition);

                if (!(swipeDistance >= swipeThreshold)) return;

                Vector2 swipeDirection = endTouchPosition - startTouchPosition;

                // Horizontal swipe
                if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
                {
                    if (swipeDirection.x > 0)
                    {
                        SendSwipeMessage(Direction.Right);
                    }
                    else
                    {
                        SendSwipeMessage(Direction.Left);
                    }
                }
                // Vertical swipe
                else
                {
                    if (swipeDirection.y > 0)
                    {
                        SendSwipeMessage(Direction.Up);
                    }
                    else
                    {
                        SendSwipeMessage(Direction.Down);
                    }
                }
            }

            void SendSwipeMessage(Direction direction)
            {
                //Debug.Log($"Swiped {direction}!");

                if (OnSwipe != null)
                {
                    OnSwipe(direction);
                }
            }

        }
    }
}