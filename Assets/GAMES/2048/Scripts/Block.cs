using TMPro;
using UnityEngine;

namespace YugantLoyaLibrary.Game2048
{
    public class Block : MonoBehaviour
    {
        public int value;
        public Node currBlockNode;
        public Vector2 Pos => transform.position;
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] private TextMeshPro blockText;

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
    }
}