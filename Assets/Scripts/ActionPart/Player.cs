using System.Threading.Tasks;
using UnityEngine;

namespace Syoujyo_no_Yume
{
    public class Player : MonoBehaviour
    {
        public enum PlayerState
        {
            Idle,
            Walk,
            Jump,
            Jumping,
            Fall,
            Landing,
        }

        public PlayerState CurrentPlayerState { get; private set; } = PlayerState.Idle;
        // �ړ����x��ݒ肵�Ă��������B
        [SerializeField]
        private float walkSpeed = 1f;
        // �W�����v�͂�ݒ肵�Ă��������B
        [SerializeField]
        private Vector2 jumpPower = new(0, 8);

        // �n�ʂƂ̐ڐG����
        [SerializeField]
        private bool isGrounded = false;
        // �n�ʃ��C���[
        [SerializeField]
        private LayerMask groundLayer = default;
        // �n�ʂƂ̐ڐG����`�F�b�J�[
        [SerializeField]
        private Transform groundChecker = null;

        private new Rigidbody2D rigidbody;

        private Animator animator;
        // Animator�̃p�����[�^�[ID
        static readonly int idleId = Animator.StringToHash("Idle");
        static readonly int walkId = Animator.StringToHash("Walk");
        static readonly int jumpId = Animator.StringToHash("Jump");
        static readonly int LandingId = Animator.StringToHash("Landing");

        private void Awake()
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

                case PlayerState.Jump:
                    UpdateJumpState();
                    break;

                case PlayerState.Jumping:
                    UpdateJumpingState();
                    break;

                case PlayerState.Fall:
                    UpdateFallState();
                    break;

                case PlayerState.Landing:
                    UpdateLandingState();
                    break;
            }
            UpdateAllState();
        }

        private void UpdateIdleState()
        {
            // ������ԂɈڍs
            if (!isGrounded)
            {
                SetFallState();
            }
            // �W�����v��ԂɈڍs
            else if (Input.GetButtonDown("Jump"))
            {
                SetJumpState();
            }
            // ���s��Ԃֈڍs
            else if (Input.GetAxis("Horizontal") != 0)
            {
                SetWalkState();
            }
        }

        private void UpdateWalkState()
        {
            // ������ԂɈڍs
            if (!isGrounded)
            {
                SetFallState();
            }
            // �W�����v��ԂɈڍs
            else if (Input.GetButtonDown("Jump"))
            {
                SetJumpState();
            }
            //�@�ҋ@��Ԃֈڍs
            else if (Input.GetAxis("Horizontal") == 0)
            {
                SetIdleState();
            }

            // �v���C���[�X�s�[�h�̐ݒ�
            float translation = Input.GetAxis("Horizontal") * walkSpeed;
            var velocity = rigidbody.velocity;
            velocity.x = translation;
            rigidbody.velocity = velocity;
        }

        private void UpdateJumpState()
        {
            // �W�����v��A�������ꂽ�ꍇ
            if (!isGrounded)
            {
                SetJumpingState();
            }
        }

        private void UpdateJumpingState()
        {
            // ���n��ԂɈڍs
            if (isGrounded)
            {
                SetLandingState();
            }

            // �v���C���[�X�s�[�h�̐ݒ�
            float translation = Input.GetAxis("Horizontal") * walkSpeed;
            var velocity = rigidbody.velocity;
            velocity.x = translation;
            rigidbody.velocity = velocity;
        }

        private void UpdateFallState()
        {
            // ���n��ԂɈڍs
            if (isGrounded)
            {
                SetLandingState();
            }

            // �v���C���[�X�s�[�h�̐ݒ�
            float translation = Input.GetAxis("Horizontal") * walkSpeed;
            var velocity = rigidbody.velocity;
            velocity.x = translation;
            rigidbody.velocity = velocity;
        }

        private async Task Landing()
        {
            await Task.Delay(200);

            // ���͂Ȃ��Œ��n
            if (Input.GetAxis("Horizontal") == 0)
            {
                SetIdleState();
            }
            // ���͂���Œ��n
            else if (Input.GetAxis("Horizontal") != 0)
            {
                SetWalkState();
            }
        }

        private void UpdateLandingState()
        {
            // �v���C���[�X�s�[�h�̐ݒ�
            float translation = Input.GetAxis("Horizontal") * walkSpeed;
            var velocity = rigidbody.velocity;
            velocity.x = translation;
            rigidbody.velocity = velocity;
        }

        private void UpdateAllState()
        {
            // �ڒn����
            isGrounded = Physics2D.OverlapBox(
                groundChecker.position,
                groundChecker.lossyScale,
                groundChecker.eulerAngles.z,
                groundLayer);

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

        // �v���C���[�������ɂ��܂��B
        public void Active()
        {
            enabled = true;
        }

        // �v���C���[��񊈐��ɂ��܂��B
        public void Inactive()
        {
            // �������Ȃ����܂��B
            var velocity = rigidbody.velocity;
            velocity.x = 0;
            rigidbody.velocity = velocity;

            if (CurrentPlayerState == PlayerState.Jumping || CurrentPlayerState == PlayerState.Jump)
            {
                animator.SetTrigger(LandingId);
            }
            else if (CurrentPlayerState == PlayerState.Idle)
            {
                enabled = false;
                return;
            }

            SetIdleState();
            enabled = false;
        }

        private void SetIdleState()
        {
            CurrentPlayerState = PlayerState.Idle;
            animator.SetTrigger(idleId);
        }

        private void SetWalkState()
        {
            CurrentPlayerState = PlayerState.Walk;
            animator.SetTrigger(walkId);
        }

        private void SetJumpState()
        {
            rigidbody.AddForce(jumpPower, ForceMode2D.Impulse);
            CurrentPlayerState = PlayerState.Jump;
            animator.SetTrigger(jumpId);
        }

        private void SetJumpingState()
        {
            CurrentPlayerState = PlayerState.Jumping;
        }

        private void SetFallState()
        {
            CurrentPlayerState = PlayerState.Fall;
            animator.SetTrigger(jumpId);
        }

        private void SetLandingState()
        {
            CurrentPlayerState = PlayerState.Landing;
            animator.SetTrigger(LandingId);

            _ = Landing();
        }
    }
}