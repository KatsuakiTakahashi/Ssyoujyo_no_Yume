using UnityEngine;

namespace Syoujyo_no_Yume
{
    public class PartController : MonoBehaviour
    {
        // アドベンチャーパートの親ゲームオブジェクトを設定してください
        [SerializeField]
        private GameObject adventurePartObject = null;
        [SerializeField]
        private Player player = null;

        public void OnAdventurePart()
        {
            adventurePartObject.SetActive(true);
            player.Inactive();
        }

        public void OnActionPart()
        {
            adventurePartObject.SetActive(false);
            player.Active();
        }
    }
}