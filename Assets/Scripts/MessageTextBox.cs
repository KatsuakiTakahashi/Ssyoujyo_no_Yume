using UnityEngine;
using UnityEngine.UI;

namespace Syoujyo_no_Yume
{
    // メソッドが呼び出されたときイメージを変更します。
    public class MessageTextBox : MonoBehaviour
    {
        private Image textBoxImage;

        // このイメージに設定する画像の種類を設定してください。
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