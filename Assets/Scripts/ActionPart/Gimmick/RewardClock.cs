using UnityEngine;

namespace Syoujyo_no_Yume
{
    public class RewardClock : MonoBehaviour
    {
        // �v���C���[��ݒ肵�Ă��������B
        [SerializeField]
        private Player player = null;

        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlaySe()
        {
            audioSource.Play();
        }

        public void StopSe()
        {
            audioSource.Stop();
        }

        public void PlayerInactive()
        {
            player.Inactive();
        }

        public void PlayerAcitve()
        {
            player.Active();
        }
    }
}