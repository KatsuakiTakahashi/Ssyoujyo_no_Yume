using UnityEngine;

namespace Syoujyo_no_Yume
{
    public class Clock : MonoBehaviour
    {
        [SerializeField]
        private Transform hand = null;

        private readonly float rotationBase = 30f;

        public void ClockwiseRotationShort()
        {
            hand.Rotate(0f, 0f, rotationBase);
        }
        public void ClockwiseRotationLonge()
        {
            hand.Rotate(0f, 0f, rotationBase * 3f);
        }

        public void CounterClockwiseRotationShort()
        {
            hand.Rotate(0f, 0f, -rotationBase);
        }

        public void CounterClockwiseRotationLonge()
        {
            hand.Rotate(0f, 0f, -rotationBase * 3f);
        }
    }
}