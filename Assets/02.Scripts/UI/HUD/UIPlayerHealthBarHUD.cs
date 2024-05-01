using UnityEngine;
using UnityEngine.Events;

using ProjectZ.Manager;

using DG.Tweening;

namespace ProjectZ.UI
{
    public class UIPlayerHealthBarHUD : UIHUDBase
    {
        [SerializeField] private ObjectPool _heartPool;

        protected override void Init()
        {
            base.Init();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            // StatsManager.Instance.OnPlayerHPEvent.AddListener((int value) =>
            // {
            //     OnHeartCallbackBySequence(value);
            // });
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            // StatsManager.Instance.OnPlayerHPEvent.RemoveListener((int value) =>
            // {
            //     OnHeartCallbackBySequence(value);
            // });
        }

        public void InitHeart(int maxHP)
        {
            // 체력은 5단위
            // 하트 하나는 20
            var heartAmount = maxHP / 20;
            var heartRemain = maxHP % 20;

            for (int index = 0; index < heartAmount; index++)
            {
                var tempHeart = _heartPool.CreateNewObject();

                if (tempHeart.TryGetComponent(out HeartObject heart))
                {
                    heart.SetHeart();

                    tempHeart.SetActive(true);
                    tempHeart.transform.SetAsLastSibling();
                }
            }
        }

        private Sequence UpdateHealthBar(int heartValue)
        {
            return DOTween.Sequence();
        }
    }
}
