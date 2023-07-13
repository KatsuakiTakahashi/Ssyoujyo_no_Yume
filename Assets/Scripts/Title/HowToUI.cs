using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace Syoujyo_no_Yume
{
    public class HowToUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject title = null;
        private Animator titleAnimator;
        private Title titleCS;

        private void Start()
        {
            titleAnimator = title.GetComponent<Animator>();
            titleCS = title.GetComponent<Title>();
        }

        private async void Update()
        {
            if (Input.anyKeyDown)
            {
                await HowToUIHide();
            }
        }

        public void HowToUIShow()
        {
            gameObject.SetActive(true);
        }

        private async Task HowToUIHide()
        {
            titleAnimator.SetTrigger("Return");
            gameObject.SetActive(false);

            await Task.Delay(1000);

            titleCS.ReturnFromHowTo();
        }
    }
}