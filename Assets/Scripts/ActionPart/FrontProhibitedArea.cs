using UnityEngine;

namespace Syoujyo_no_Yume
{
    // ���݂̐i�s�󋵂ɑΉ������ʒu�Ɉړ�����O����������֎~�G���A�ł��B
    public class FrontProhibitedArea : MonoBehaviour
    {
        // �Q�[���R���g���[���[��ݒ肵�Ă��������B
        [SerializeField]
        private GameController gameController = null;

        // �v���C���[��ݒ肵�Ă��������B
        [SerializeField]
        private GameObject player = null;
        Player playerCS;

        // ��������֎~�G���A�ɐN�������ۂǂ��Ɉړ������邩�ݒ肵�Ă��������B
        [SerializeField]
        private Vector2 offset = new(15f, 0f);

        // �ړ���̈ʒu��ݒ肵�Ă��������B
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