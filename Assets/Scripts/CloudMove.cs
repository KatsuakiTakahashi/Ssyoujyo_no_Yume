using System.Collections.Generic;
using UnityEngine;

namespace Syoujyo_no_Yume
{
    public class CloudMove : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> cloudObjects = new();

        // 雲が動く速度を設定してください。
        [SerializeField]
        private float moveSpeed = 0.05f;
        // どこまで離れたらリセットするか設定してください。
        [SerializeField]
        private float resetPositionX = 40f;

        private void Update()
        {
            UpdateObjectPosition();
        }

        // すべての雲の位置を更新します。
        void UpdateObjectPosition()
        {
            // リストの雲を一定に動かします。
            for (int cloudObjectIndex = 0; cloudObjectIndex < cloudObjects.Count; cloudObjectIndex++)
            {
                var position = cloudObjects[cloudObjectIndex].transform.position;
                position.x -= moveSpeed / 100;
                cloudObjects[cloudObjectIndex].transform.position = position;

                // カメラから設定した値離れるとポジションを戻します。
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