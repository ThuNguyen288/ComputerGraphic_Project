using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kinnly
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Ysort : MonoBehaviour
    {
        SpriteRenderer spriteRenderer;

        [Header("Core")]
        [SerializeField] float Offset = 0f;
        [SerializeField] int scale = 10; // Để giá trị dương để tránh lỗi khi y < 0.

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            // Sử dụng giá trị âm của vị trí y nhưng giữ sortingOrder luôn dương.
            float adjustedY = transform.position.y - Offset;
            spriteRenderer.sortingOrder = Mathf.RoundToInt(-adjustedY * scale);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - Offset, transform.position.z), 0.1f);
        }
    }
}
