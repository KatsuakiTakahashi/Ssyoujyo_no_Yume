using UnityEngine;

namespace Syoujyo_no_Yume
{
    public class PartController : MonoBehaviour
    {
        // アドベンチャーパートの親ゲームオブジェクトを設定してください
        [SerializeField]
        GameObject adventurePartObject = null;
        [SerializeField]
        Player player = null;

        private void Update()
        {
            if (adventurePartObject.activeSelf)
            {
                if (player.enabled)
                {
                    player.enabled = false;
                }
            }
            else
            {
                if (!player.enabled)
                {
                    player.enabled = true;
                }
            }
        }

        public void OnActionPart()
        {
            adventurePartObject.SetActive(true);
        }
    }
}