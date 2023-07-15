using UnityEngine;
using UnityEngine.SceneManagement;

namespace Syoujyo_no_Yume
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        DialogueScript.DialogueScript dialogue = null;

        [SerializeField]
        private Player player = null;
        [SerializeField]
        private PauseUI pauseUI = null;
        [SerializeField]
        FrontProhibitedArea frontProhibitedArea = null;

        private enum GameMode
        {
            Adventure,
            Action,
        }
        private GameMode currentGameMode = GameMode.Adventure;

        private bool isPause = false;

        public int KeyItemCount { get; private set; } = 0;

        private void Update()
        {
            if (currentGameMode == GameMode.Action)
            {
                if (Input.GetButtonDown("Cancel") && !isPause)
                {
                    Pause();
                }
                else if (Input.GetButtonDown("Cancel") && isPause)
                {
                    Resume();
                }
            }
        }

        public void ActionPart()
        {
            currentGameMode = GameMode.Action;
            dialogue.SetActive("false");
            player.Active();
        }

        private void AdventurePart()
        {
            currentGameMode = GameMode.Adventure;
            dialogue.SetActive("true");
            player.Inactive();
        }

        public void GetKeyItem()
        {
            frontProhibitedArea.NextAreaPosi();
            KeyItemCount++;
            AdventurePart();
        }

        private void Pause()
        {
            isPause = true;
            player.Inactive();
            pauseUI.Show();
        }

        public void Resume()
        {
            isPause = false;
            player.Active();
            pauseUI.Hide();
        }

        public void Retry()
        {
            SceneManager.LoadScene("Main");
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}