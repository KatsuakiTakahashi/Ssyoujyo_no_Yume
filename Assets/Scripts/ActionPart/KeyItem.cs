using UnityEngine;

namespace Syoujyo_no_Yume
{
    public class KeyItem : MonoBehaviour
    {
        [SerializeField]
        private GameObject player = null;
        Player playerCs;

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