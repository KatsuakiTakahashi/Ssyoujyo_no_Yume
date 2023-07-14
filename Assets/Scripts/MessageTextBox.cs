using UnityEngine;
using UnityEngine.UI;

namespace Syoujyo_no_Yume
{
    public class MessageTextBox : MonoBehaviour
    {
        private Image textBoxImage;

        [SerializeField]
        private Sprite[] textBoxSprites = null;

        private void Awake()
        {
            textBoxImage = GetComponent<Image>();
        }

        public void SetSprite(int spriteIndex)
        {
            textBoxImage.sprite = textBoxSprites[spriteIndex];
        }
    }
}