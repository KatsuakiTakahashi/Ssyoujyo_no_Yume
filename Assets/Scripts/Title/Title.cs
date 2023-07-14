using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Syoujyo_no_Yume
{
    public class Title : MonoBehaviour
    {
        [SerializeField]
        private Button[] buttons = null;

        [SerializeField]
        private HowToUI howToUI = null;

        private Animator animator;
        private string onButtonType = "";

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void OnButtonClick(string buttonType)
        {
            onButtonType = buttonType;

            SetButtonsInteractable(false);
            animator.SetTrigger("FadeOut");
        }

        public void FadeOutEnd()
        {
            switch (onButtonType)
            {
                case "Start":
                    LoadScene();
                    break;

                case "HowTo":
                    howToUI.HowToUIShow();
                    break;

                case "Exit":
                    Exit();
                    break;
            }
        }

        private void LoadScene()
        {
            SceneManager.LoadScene("Main");
        }

        private void Exit()
        {
            Application.Quit();
        }

        public void ReturnFromHowTo()
        {
            SetButtonsInteractable(true);
            buttons[1].Select();
        }

        private void SetButtonsInteractable(bool interactable)
        {
            foreach (Button button in buttons)
            {
                button.interactable = interactable;
            }
        }
    }
}