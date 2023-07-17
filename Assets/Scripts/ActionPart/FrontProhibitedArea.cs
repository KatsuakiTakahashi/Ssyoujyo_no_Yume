using UnityEngine;

namespace Syoujyo_no_Yume
{
    // 現在の進行状況に対応した位置に移動する前方立ち入り禁止エリアです。
    public class FrontProhibitedArea : MonoBehaviour
    {
        // ゲームコントローラーを設定してください。
        [SerializeField]
        private GameController gameController = null;

        // プレイヤーを設定してください。
        [SerializeField]
        private GameObject player = null;
        Player playerCS;

        // 立ち入り禁止エリアに侵入した際どこに移動させるか設定してください。
        [SerializeField]
        private Vector2 offset = new(15f, 0f);

        // 移動先の位置を設定してください。
        [SerializeField]
        private Vector2[] setPositions = null;

        private Animator animator;

        private void Start()
        {
            playerCS = player.GetComponent<Player>();
            animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == player)
            {
                playerCS.Inactive();
                animator.SetTrigger("Enter");
            }
        }

        public void NextAreaPosi()
        {
            var position = setPositions[gameController.KeyItemCount];
            transform.position = position;
        }

        public void PlayerBackMove()
        {
            var position = player.transform.position;
            position.x = player.transform.position.x - offset.x / 2;
            player.transform.position = position;

            var localScale = transform.localScale;
            localScale.x = player.transform.localScale.x * -1;
            player.transform.localScale = localScale;
        }

        public void PlayerActive()
        {
            playerCS.Active();
        }
    }
}