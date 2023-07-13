using UnityEngine;

namespace Syoujyo_no_Yume
{
    public class PopMushrooms : MonoBehaviour
    {
        // プレイヤーを設定してください。
        [SerializeField]
        GameObject player = null;
        private Rigidbody2D playerRb;

        // 反発力を設定してください。
        [SerializeField]
        float repellentForce = 1f;

        private void Start()
        {
            playerRb = player.GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // 衝突した相手がプレイヤーであるかチェック
            if (collision.gameObject == player)
            {               
                // 衝突した相手の法線ベクトルを取得
                Vector2 contactNormal = -collision.contacts[0].normal;

                playerRb.AddForce(contactNormal * repellentForce, ForceMode2D.Impulse);
            }
        }
    }
}