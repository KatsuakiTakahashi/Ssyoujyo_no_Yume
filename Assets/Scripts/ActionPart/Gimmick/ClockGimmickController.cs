using UnityEngine;

namespace Syoujyo_no_Yume
{
    // ‘ÎÛ‚ª³‰ð‚Ìó‘Ô‚©‚Ç‚¤‚©”»•Ê‚µA³‰ð‚È‚ç•ñV‚ðo‚µ‚Ü‚·B
    public class ClockGimmickController : MonoBehaviour
    {
        // oŒ»‚³‚¹‚é•ñV‚ðÝ’è‚µ‚Ä‚­‚¾‚³‚¢B
        [SerializeField]
        private GameObject clearReward = null;

        // ”»’è‘ÎÛ‚P‚ðÝ’è‚µ‚Ä‚­‚¾‚³‚¢B
        [SerializeField]
        private Transform clock1Hand = null;
        // ‘ÎÛ‚P‚Ì³‰ð‚Ìó‘Ô‚ðÝ’è‚µ‚Ä‚­‚¾‚³‚¢B
        [SerializeField]
        private float clock1CorrectRotation = 240f;

        // ”»’è‘ÎÛ‚Q‚ðÝ’è‚µ‚Ä‚­‚¾‚³‚¢B
        [SerializeField]
        private Transform clock2Hand = null;
        // ‘ÎÛ‚Q‚Ì³‰ð‚Ìó‘Ô‚ðÝ’è‚µ‚Ä‚­‚¾‚³‚¢B
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

        // ³‰ð‚Ìó‘Ô‚©”»’è‚µA³‰ð‚Ìê‡•ñV‚ðo‚µ‚Ü‚·B
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