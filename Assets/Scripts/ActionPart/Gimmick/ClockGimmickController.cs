using UnityEngine;

namespace Syoujyo_no_Yume
{
    public class ClockGimmickController : MonoBehaviour
    {
        [SerializeField]
        private GameObject clearReward = null;

        [SerializeField]
        private Transform clock1Hand = null;
        [SerializeField]
        private float clock1CorrectRotation = 240f;

        [SerializeField]
        private Transform clock2Hand = null;
        [SerializeField]
        private float clock2CorrectRotation = 0f;

        private void Start()
        {
            if (clearReward.activeSelf)
            {
                clearReward.SetActive(false);
            }
        }

        public void ClearCheck()
        {
            if (clock1CorrectRotation + 20 > clock1Hand.eulerAngles.z && clock1Hand.eulerAngles.z > clock1CorrectRotation - 20)
            {
                if (clock2CorrectRotation + 20 > clock2Hand.eulerAngles.z && clock2Hand.eulerAngles.z > clock2CorrectRotation - 20)
                {
                    clearReward.SetActive(true);
                }
            }
        }
    }
}