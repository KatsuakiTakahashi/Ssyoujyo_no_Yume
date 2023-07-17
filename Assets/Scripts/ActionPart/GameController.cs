using UnityEngine;
using UnityEngine.SceneManagement;

namespace Syoujyo_no_Yume
{
    // ゲームパートを切替て現在の状態を保持します。
    public class GameController : MonoBehaviour
    {
        // アドベンチャーパート管理者を設定してください。
        [SerializeField]
        DialogueScript.DialogueScript dialogue = null;

        // プレイヤーを設定してください。
        [SerializeField]
        private Player player = null;

        // ポーズUIを設定してください。
        [SerializeField]
        private PauseUI pauseUI = null;
        // 前方の立ち入り禁止エリアを設定してください。
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