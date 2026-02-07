using DotsRts.Systems;
using UnityEngine;

namespace DotsRts.MonoBehaviours
{
    public class GridSystemDebugSingle : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private int _x;
        private int _y;

        public void Setup(int x, int y, float gridNodeSize)
        {
            _x = x;
            _y = y;

            transform.position = GridSystem.GetWorldPosition(x, y, gridNodeSize);
        }

        public void SetColor(Color color)
        {
            _spriteRenderer.color = color;
        }
    }
}