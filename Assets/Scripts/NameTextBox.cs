using UnityEngine;
using UnityEngine.UI;

namespace Syoujyo_no_Yume
{
    // 起動時にこのオブジェクトを非表示にし、メソッドが呼び出されたときイメージを変更します。
    public class NameTextBox : MonoBehaviour
    {
        private Image textBoxImage;

        // このイメージに設定する画像の種類を設定してください。
        [SerializeField]
        private Sprite[] textBoxSprites = null;

        private void Awake()
        {
            textBoxImage = GetComponent<Image>();
            gameObject.SetActive(false);
        }

        public void SetSprite(int spriteIndex)
        {
            textBoxImage.sprite = textBoxSprites[spriteIndex];
        }
    }
}