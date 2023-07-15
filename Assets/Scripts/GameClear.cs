using UnityEngine;
using UnityEngine.SceneManagement;

namespace Syoujyo_no_Yume
{
    public class GameClear : MonoBehaviour
    {
        public void ReturnTitle()
        {
            SceneManager.LoadScene("Title");
        }
    }
}