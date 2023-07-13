using UnityEngine;

namespace Syoujyo_no_Yume
{
    // 背景の連続スクロールを制御します。
    [ExecuteInEditMode]
    public class ScrollBackground : MonoBehaviour
    {
        // スプライト画像を配列で指定します。
        [SerializeField]
        private Transform[] sprites = null;

        // 背景スプライト一枚当たりのUnitサイズ
        Vector3 gridSize;

        // Start is called before the first frame update
        void Start()
        {
            // 0番目の画像から表示サイズ（Unit単位）を取得
            gridSize = sprites[0].GetComponent<SpriteRenderer>().bounds.size;

            UpdateSprites();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateSprites();
        }

        // すべてのパネルの位置を更新します。
        void UpdateSprites()
        {
            // カメラの位置
            var cameraGridX = Mathf.FloorToInt(Camera.main.transform.position.x / gridSize.x);

            // 配列の画像を並べる
            for (int index = 0; index < sprites.Length; index++)
            {
                var position = sprites[index].position;
                position.x = (index - 1 + cameraGridX) * gridSize.x;
                sprites[index].position = position;
            }
        }
    }
}