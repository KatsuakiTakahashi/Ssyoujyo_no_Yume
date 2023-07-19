using UnityEngine;

namespace Syoujyo_no_Yume
{
    public class GearRotation : MonoBehaviour
    {
        [SerializeField]
        private Gear[] gears=null;

        private void Update()
        {
            UpdateGearRotation();
        }

        private void UpdateGearRotation()
        {
            for (int gearIndex = 0; gearIndex < gears.Length; gearIndex++)
            {
                gears[gearIndex].transform.Rotate(0, 0, gears[gearIndex].rotationSpeed);
            }
        }
    }

    [System.Serializable]
    public class Gear
    {
        [SerializeField]
        public Transform transform = null;

        [SerializeField]
        public float rotationSpeed = 1.0f;
    }
}