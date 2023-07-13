using UnityEngine;

namespace Syoujyo_no_Yume
{
    public class ChaseCamera : MonoBehaviour
    {
        // 追尾する対象を指定します。
        [SerializeField]
        private Transform target = null;
        // 追尾対象からのオフセット値を指定します。
        [SerializeField]
        private Vector2 offset = new(0f, 1.5f);

        private void Start()
        {
            TargetChase();
        }

        private void Update()
        {
            TargetChase();
        }

        // ターゲットの位置に移動します。
        private void TargetChase()
        {
            var position = transform.position;
            position.x = target.position.x + offset.x;
            transform.position = position;
        }
    }
}