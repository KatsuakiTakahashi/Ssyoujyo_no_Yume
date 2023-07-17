using UnityEngine;
using UnityEngine.UI;

namespace Syoujyo_no_Yume
{
    // ���\�b�h���Ăяo���ꂽ�Ƃ��C���[�W��ύX���܂��B
    public class MessageTextBox : MonoBehaviour
    {
        private Image textBoxImage;

        // ���̃C���[�W�ɐݒ肷��摜�̎�ނ�ݒ肵�Ă��������B
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