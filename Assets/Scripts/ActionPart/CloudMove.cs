using System.Collections.Generic;
using UnityEngine;

namespace Syoujyo_no_Yume
{
    public class CloudMove : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> cloudObjects = new();

        // �_���������x��ݒ肵�Ă��������B
        [SerializeField]
        private float moveSpeed = 0.05f;
        // �ǂ��܂ŗ��ꂽ�烊�Z�b�g���邩�ݒ肵�Ă��������B
        [SerializeField]
        private float resetPositionX = 40f;

        private void Update()
        {
            UpdateObjectPosition();
        }

        // ���ׂẲ_�̈ʒu���X�V���܂��B
        void UpdateObjectPosition()
        {
            // ���X�g�̉_�����ɓ������܂��B
            for (int cloudObjectIndex = 0; cloudObjectIndex < cloudObjects.Count; cloudObjectIndex++)
            {
                var position = cloudObjects[cloudObjectIndex].transform.position;
                position.x -= moveSpeed / 100;
                cloudObjects[cloudObjectIndex].transform.position = position;

                // �J��������ݒ肵���l�����ƃ|�W�V������߂��܂��B
                if (cloudObjects[cloudObjectIndex].transform.position.x < Camera.main.transform.position.x - resetPositionX)
                {
                    position = cloudObjects[cloudObjectIndex].transform.position;
                    position.x = Camera.main.transform.position.x + resetPositionX;
                    cloudObjects[cloudObjectIndex].transform.position = position;
                }
            }
        }
    }
}