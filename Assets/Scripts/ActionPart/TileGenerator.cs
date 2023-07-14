using UnityEngine;

namespace Syoujyo_no_Yume
{
    public class TileGenerator : MonoBehaviour
    {
        // 生成するオブジェクトのプレハブ
        [SerializeField]
        private GameObject tilePrefab = null;
        // 生成する個数
        [SerializeField]
        private int numberOTiles = 0;

        private void Start()
        {
            GenerateTiles();
        }

        // 指定された個数オブレジェンドを生成します。
        private void GenerateTiles()
        {
            for (int i = 0; i < numberOTiles; i++)
            {
                // 1ユニットずつずらして生成します。
                Vector3 position = new(i * 1f, 0f, 0f);
                Quaternion rotation = Quaternion.identity;

                // オブジェクトを生成
                GameObject newObject = Instantiate(tilePrefab, position, rotation);

                // 子オブジェクトにして位置を修正します。
                newObject.transform.SetParent(transform);
                newObject.transform.localPosition = position;
                newObject.transform.localRotation = rotation;
            }
        }
    }
}