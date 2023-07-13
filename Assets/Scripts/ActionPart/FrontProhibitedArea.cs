using UnityEngine;

namespace Syoujyo_no_Yume
{
    public class FrontProhibitedArea : MonoBehaviour
    {
        [SerializeField]
        private ActionPartController actionPartController = null;

        [SerializeField]
        private GameObject player = null;
        Player playerCS;

        [SerializeField]
        private Vector2 offset = new(15f, 0f);

        [SerializeField]
        private Vector2[] arePositions = null;

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
            var position = transform.position;
            position = arePositions[actionPartController.KeyItemCount];
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