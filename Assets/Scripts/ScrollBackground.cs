using UnityEngine;

namespace Syoujyo_no_Yume
{
    // �w�i�̘A���X�N���[���𐧌䂵�܂��B
    [ExecuteInEditMode]
    public class ScrollBackground : MonoBehaviour
    {
        // �X�v���C�g�摜��z��Ŏw�肵�܂��B
        [SerializeField]
        private Transform[] sprites = null;

        // �w�i�X�v���C�g�ꖇ�������Unit�T�C�Y
        Vector3 gridSize;

        // Start is called before the first frame update
        void Start()
        {
            // 0�Ԗڂ̉摜����\���T�C�Y�iUnit�P�ʁj���擾
            gridSize = sprites[0].GetComponent<SpriteRenderer>().bounds.size;

            UpdateSprites();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateSprites();
        }

        // ���ׂẴp�l���̈ʒu���X�V���܂��B
        void UpdateSprites()
        {
            // �J�����̈ʒu
            var cameraGridX = Mathf.FloorToInt(Camera.main.transform.position.x / gridSize.x);

            // �z��̉摜����ׂ�
            for (int index = 0; index < sprites.Length; index++)
            {
                var position = sprites[index].position;
                position.x = (index - 1 + cameraGridX) * gridSize.x;
                sprites[index].position = position;
            }
        }
    }
}