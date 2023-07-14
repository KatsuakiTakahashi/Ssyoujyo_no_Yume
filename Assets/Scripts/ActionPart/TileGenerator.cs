using UnityEngine;

namespace Syoujyo_no_Yume
{
    public class TileGenerator : MonoBehaviour
    {
        // ��������I�u�W�F�N�g�̃v���n�u
        [SerializeField]
        private GameObject tilePrefab = null;
        // ���������
        [SerializeField]
        private int numberOTiles = 0;

        private void Start()
        {
            GenerateTiles();
        }

        // �w�肳�ꂽ���I�u���W�F���h�𐶐����܂��B
        private void GenerateTiles()
        {
            for (int i = 0; i < numberOTiles; i++)
            {
                // 1���j�b�g�����炵�Đ������܂��B
                Vector3 position = new(i * 1f, 0f, 0f);
                Quaternion rotation = Quaternion.identity;

                // �I�u�W�F�N�g�𐶐�
                GameObject newObject = Instantiate(tilePrefab, position, rotation);

                // �q�I�u�W�F�N�g�ɂ��Ĉʒu���C�����܂��B
                newObject.transform.SetParent(transform);
                newObject.transform.localPosition = position;
                newObject.transform.localRotation = rotation;
            }
        }
    }
}