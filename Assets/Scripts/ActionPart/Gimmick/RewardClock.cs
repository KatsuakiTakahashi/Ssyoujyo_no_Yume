using UnityEngine;

namespace Syoujyo_no_Yume
{
    public class RewardClock : MonoBehaviour
    {
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
    }
}