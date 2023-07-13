using UnityEngine;

namespace Syoujyo_no_Yume
{
    public class PartController : MonoBehaviour
    {
        // �A�h�x���`���[�p�[�g�̐e�Q�[���I�u�W�F�N�g��ݒ肵�Ă�������
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