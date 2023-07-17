using UnityEngine;
using UnityEngine.UI;

namespace Syoujyo_no_Yume
{
    // プレイヤーが侵入するとUIを表示し、それぞれのボタンに対応したアニメーションが再生されます。
    public class CheckArea : MonoBehaviour
    {
        [SerializeField]
        private GameObject player = null;
        Player playerCS;

        [SerializeField]
        private Vector2 offset = new(-10f, 0f);

        [SerializeField]
        private Button firstSelected=null;

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

        public void ButtonSelect()
        {
            firstSelected.Select();
        }

        public void Clear()
        {
            animator.SetTrigger("Clear");
        }

        public void MissTake()
        {
            animator.SetTrigger("MissTake");
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