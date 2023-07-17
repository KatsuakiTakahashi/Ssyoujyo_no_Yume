using UnityEngine;

namespace Syoujyo_no_Yume
{
    // �Ώۂ������̏�Ԃ��ǂ������ʂ��A�����Ȃ��V���o���܂��B
    public class ClockGimmickController : MonoBehaviour
    {
        // �o���������V��ݒ肵�Ă��������B
        [SerializeField]
        private GameObject clearReward = null;

        // ����ΏۂP��ݒ肵�Ă��������B
        [SerializeField]
        private Transform clock1Hand = null;
        // �ΏۂP�̐����̏�Ԃ�ݒ肵�Ă��������B
        [SerializeField]
        private float clock1CorrectRotation = 240f;

        // ����ΏۂQ��ݒ肵�Ă��������B
        [SerializeField]
        private Transform clock2Hand = null;
        // �ΏۂQ�̐����̏�Ԃ�ݒ肵�Ă��������B
        [SerializeField]
        private float clock2CorrectRotation = 0f;

        private bool isClear = false;

        private void Start()
        {
            if (!isClear)
            {
                clearReward.SetActive(false);
            }
        }

        // �����̏�Ԃ����肵�A�����̏ꍇ��V���o���܂��B
        public void ClearCheck()
        {
            if (clock1CorrectRotation + 20 > clock1Hand.eulerAngles.z && clock1Hand.eulerAngles.z > clock1CorrectRotation - 20)
            {
                if (clock2CorrectRotation + 20 > clock2Hand.eulerAngles.z && clock2Hand.eulerAngles.z > clock2CorrectRotation - 20)
                {
                    clearReward.SetActive(true);
                    isClear = true;
                }
            }
        }
    }
}