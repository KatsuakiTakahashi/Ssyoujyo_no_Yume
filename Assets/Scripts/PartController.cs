using UnityEngine;

namespace Syoujyo_no_Yume
{
    public class PartController : MonoBehaviour
    {
        // �A�h�x���`���[�p�[�g�̐e�Q�[���I�u�W�F�N�g��ݒ肵�Ă�������
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