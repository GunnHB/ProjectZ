using UnityEngine;
using UnityEngine.UI;

namespace ProjectZ.UI
{
    public class HeartObject : MonoBehaviour
    {
        public enum HeartType
        {
            None = -1,
            Empty,
            AQuarter,
            Half,
            ThreeQaurter,
            Full,
        }

        [SerializeField] private Image _normalHeart;

        public bool IsFull => _normalHeart.fillAmount == 1f;

        private HeartType _heartType;

        public void SetHeart(HeartType type)
        {
            _heartType = type;

            FillHeartByType();
        }

        private void FillHeartByType()
        {
            if (_heartType == HeartType.None)
                return;

            switch (_heartType)
            {
                case HeartType.Empty:
                    _normalHeart.fillAmount = 0f;
                    break;
                case HeartType.AQuarter:
                    _normalHeart.fillAmount = .25f;
                    break;
                case HeartType.Half:
                    _normalHeart.fillAmount = .5f;
                    break;
                case HeartType.ThreeQaurter:
                    _normalHeart.fillAmount = .75f;
                    break;
                case HeartType.Full:
                    _normalHeart.fillAmount = 1f;
                    break;
            }
        }
    }
}
