using UnityEngine;

namespace Syoujyo_no_Yume
{
    public class ActionPartController : MonoBehaviour
    {
        [SerializeField]
        DialogueScript.DialogueScript dialogue = null;

        [SerializeField]
        private Player player = null;
        [SerializeField]
        FrontProhibitedArea frontProhibitedArea = null;

        public int KeyItemCount { get; private set; } = 0;

        public void GetKeyItem()
        {
            frontProhibitedArea.NextAreaPosi();
            KeyItemCount++;
            player.Inactive();
            dialogue.SetActive("true");
        }
    }
}