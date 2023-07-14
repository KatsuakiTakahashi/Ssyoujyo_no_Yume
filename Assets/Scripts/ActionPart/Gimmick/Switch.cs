using UnityEngine;
using UnityEngine.Events;

namespace Syoujyo_no_Yume
{
    public class Switch : MonoBehaviour
    {
        // �v���C���[
        [SerializeField]
        private GameObject player = null;
        [SerializeField]
        private UnityEvent onSwitchEvent = null;

        private Animator animator;
        static readonly int activeId = Animator.StringToHash("Active");
        static readonly int inactiveId = Animator.StringToHash("Inactive");

        AudioSource audioSource;

        private void Start()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }

        // �v���C���[���ڐG���Ă����Ƃ��ɔ���
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == player)
            {
                audioSource.Play();
                animator.SetTrigger(activeId);
                onSwitchEvent.Invoke();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject == player)
            {
                animator.SetTrigger(inactiveId);
            }
        }
    }
}