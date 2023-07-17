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
        // 移動速度を設定してください。
        [SerializeField]
        private float walkSpeed = 1f;
        // ジャンプ力を設定してください。
        [SerializeField]
        private Vector2 jumpPower = new(0, 8);

        // 地面との接触判定
        [SerializeField]
        private bool isGrounded = false;
        // 地面レイヤー
        [SerializeField]
        private LayerMask groundLayer = default;
        // 地面との接触判定チェッカー
        [SerializeField]
        private Transform groundChecker = null;

        private new Rigidbody2D rigidbody;

        private Animator animator;
        // AnimatorのパラメーターID
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
            // 落下状態に移行
            if (!isGrounded)
            {
                SetFallState();
            }
            // ジャンプ状態に移行
            else if (Input.GetButtonDown("Jump"))
            {
                SetJumpState();
            }
            // 歩行状態へ移行
            else if (Input.GetAxis("Horizontal") != 0)
            {
                SetWalkState();
            }
        }

        private void UpdateWalkState()
        {
            // 落下状態に移行
            if (!isGrounded)
            {
                SetFallState();
            }
            // ジャンプ状態に移行
            else if (Input.GetButtonDown("Jump"))
            {
                SetJumpState();
            }
            //　待機状態へ移行
            else if (Input.GetAxis("Horizontal") == 0)
            {
                SetIdleState();
            }

            // プレイヤースピードの設定
            float translation = Input.GetAxis("Horizontal") * walkSpeed;
            var velocity = rigidbody.velocity;
            velocity.x = translation;
            rigidbody.velocity = velocity;
        }

        private void UpdateJumpState()
        {
            // ジャンプ後、足が離れた場合
            if (!isGrounded)
            {
                SetJumpingState();
            }
        }

        private void UpdateJumpingState()
        {
            // 着地状態に移行
            if (isGrounded)
            {
                SetLandingState();
            }

            // プレイヤースピードの設定
            float translation = Input.GetAxis("Horizontal") * walkSpeed;
            var velocity = rigidbody.velocity;
            velocity.x = translation;
            rigidbody.velocity = velocity;
        }

        private void UpdateFallState()
        {
            // 着地状態に移行
            if (isGrounded)
            {
                SetLandingState();
            }

            // プレイヤースピードの設定
            float translation = Input.GetAxis("Horizontal") * walkSpeed;
            var velocity = rigidbody.velocity;
            velocity.x = translation;
            rigidbody.velocity = velocity;
        }

        private async Task Landing()
        {
            await Task.Delay(200);

            // 入力なしで着地
            if (Input.GetAxis("Horizontal") == 0)
            {
                SetIdleState();
            }
            // 入力ありで着地
            else if (Input.GetAxis("Horizontal") != 0)
            {
                SetWalkState();
            }
        }

        private void UpdateLandingState()
        {
            // プレイヤースピードの設定
            float translation = Input.GetAxis("Horizontal") * walkSpeed;
            var velocity = rigidbody.velocity;
            velocity.x = translation;
            rigidbody.velocity = velocity;
        }

        private void UpdateAllState()
        {
            // 接地判定
            isGrounded = Physics2D.OverlapBox(
                groundChecker.position,
                groundChecker.lossyScale,
                groundChecker.eulerAngles.z,
                groundLayer);

            // Horizontalの状況を取得
            float moveHorizontal = Input.GetAxis("Horizontal");

            if (moveHorizontal != 0)
            {
                // キャラクターの向き制御
                Vector2 lscale = gameObject.transform.localScale;
                if ((lscale.x > 0 && moveHorizontal < 0)
                    || (lscale.x < 0 && moveHorizontal > 0))
                {
                    lscale.x *= -1;
                    gameObject.transform.localScale = lscale;
                }
            }
        }

        // プレイヤーを活性にします。
        public void Active()
        {
            enabled = true;
        }

        // プレイヤーを非活性にします。
        public void Inactive()
        {
            // 慣性をなくします。
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