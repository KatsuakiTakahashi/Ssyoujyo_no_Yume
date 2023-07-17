using UnityEngine;

namespace Syoujyo_no_Yume
{
    // ���̃I�u�W�F�N�g�Ƀv���C���[���ڐG����ƃA�h�x���`���[�p�[�g�Ɉڍs���܂��B
    public class KeyItem : MonoBehaviour
    {
        // �v���C���[�I�u�W�F�N�g��ݒ肵�Ă��������B
        [SerializeField]
        private GameObject player = null;
        Player playerCs;

        // �Q�[���R���g���[���[��ݒ肵�Ă��������B
        [SerializeField]
        private GameController gameController = null;

        Animator animator;

        private void Start()
        {
            playerCs = player.GetComponent<Player>();
            animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == player)
            {
                playerCs.Inactive();
                animator.SetTrigger("Get");
            }
        }

        public void GetItem()
        {
            gameObject.SetActive(false);
            gameController.GetKeyItem();
        }
    }
}