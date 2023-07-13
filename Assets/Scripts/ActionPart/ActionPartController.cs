using UnityEngine;

namespace Syoujyo_no_Yume
{
    public class ActionPartController : MonoBehaviour
    {
        [SerializeField]
        PartController partController = null;

        [SerializeField]
        FrontProhibitedArea frontProhibitedArea = null;

        public int KeyItemCount { get; private set; } = 0;

        public void GetKeyItem()
        {
            frontProhibitedArea.NextAreaPosi();
            KeyItemCount++;
            partController.OnAdventurePart();
        }
    }
}