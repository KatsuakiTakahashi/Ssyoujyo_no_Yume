using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Syoujyo_no_Yume
{
    public class Player : MonoBehaviour
    {
        public enum PlayerState
        {
            Idle,
            Walk,
            Fall,
            Jump,
            Landing,
        }
        public PlayerState CurrentPlayerState { get; private set; } = PlayerState.Idle;
        [SerializeField]
        private float walkSpeed = 1f;
        [SerializeField]
        private Vector2 jumpPower = new(0,8);

        private new Rigidbody2D rigidbody;

        private Animator animator;
        // Animator�̃p�����[�^�[ID
        static readonly int idleId = Animator.StringToHash("Idle");
        static readonly int walkId = Animator.StringToHash("Walk");
        static readonly int jumpId = Animator.StringToHash("Jump");
        static readonly int landingId = Animator.StringToHash("Landing");

        private void Start()
        {
            animator = GetComponentInChildren<Animator>();
            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            switch (CurrentPlayerState)
            {
                case PlayerState.Idle:
                    UpdateIdleState();
                    break;

                    case PlayerState.Walk:
                    UpdateWalkState();
                    break;
            }
            UpdateAllState();
        }

        private void UpdateIdleState()
        {
            // ���s��Ԃֈڍs
            if (Input.GetAxis("Horizontal") != 0)
            {
                CurrentPlayerState = PlayerState.Walk;
                animator.SetTrigger(walkId);
            }
        }

        private void UpdateWalkState()
        {
             //�@�ҋ@��Ԃֈڍs
            if (Input.GetAxis("Horizontal") == 0)
            {
                CurrentPlayerState = PlayerState.Idle;
                animator.SetTrigger(idleId);
            }

            // �v���C���[�X�s�[�h�̐ݒ�
            float translation = Input.GetAxis("Horizontal") * walkSpeed;
            var velocity = rigidbody.velocity;
            velocity.x = translation;
            rigidbody.velocity = velocity;
        }

        private void UpdateAllState()
        {
            // Horizontal�̏󋵂��擾
            float moveHorizontal = Input.GetAxis("Horizontal");

            if (moveHorizontal != 0)
            {
                // �L�����N�^�[�̌�������
                Vector2 lscale = gameObject.transform.localScale;
                if ((lscale.x > 0 && moveHorizontal < 0)
                    || (lscale.x < 0 && moveHorizontal > 0))
                {
                    lscale.x *= -1;
                    gameObject.transform.localScale = lscale;
                }
            }
        }
    }
}