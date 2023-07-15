using UnityEngine;
using UnityEngine.UI;

namespace Syoujyo_no_Yume
{
    public class PauseUI : MonoBehaviour
    {
        [SerializeField]
        private Button firstSelected = null;

        private void Start()
        {
            if (gameObject)
            {
                gameObject.SetActive(false);
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
            firstSelected.Select();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}