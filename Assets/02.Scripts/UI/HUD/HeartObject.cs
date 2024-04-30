using UnityEngine;
using UnityEngine.UI;

namespace ProjectZ.UI
{
    public class HeartObject : MonoBehaviour
    {
        [SerializeField] private Image _normalHeart;

        public void SetHeart(float fillAmount = 1f)
        {
            _normalHeart.fillAmount = fillAmount;
        }
    }
}
