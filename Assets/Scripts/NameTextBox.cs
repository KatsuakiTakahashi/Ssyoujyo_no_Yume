using UnityEngine;
using UnityEngine.UI;

namespace Syoujyo_no_Yume
{
    // �N�����ɂ��̃I�u�W�F�N�g���\���ɂ��A���\�b�h���Ăяo���ꂽ�Ƃ��C���[�W��ύX���܂��B
    public class NameTextBox : MonoBehaviour
    {
        private Image textBoxImage;

        // ���̃C���[�W�ɐݒ肷��摜�̎�ނ�ݒ肵�Ă��������B
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