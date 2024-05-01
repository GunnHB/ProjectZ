using UnityEngine;
using UnityEngine.Events;

using ProjectZ.Manager;
using System;

namespace ProjectZ.UI
{
    public class UIPlayerHealthBarHUD : UIHUDBase
    {
        [SerializeField] private ObjectPool _heartPool;

        private Core.Characters.PlayerStats _playerStats;
        private Coroutine _fillHeartCoroutine;

        public UnityAction<int> OnHealthBarAction;

        private System.Collections.Generic.List<HeartObject> _heartList = new();

        protected override void Init()
        {
            base.Init();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            OnHealthBarAction += StartFillHeartCoroutine;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            OnHealthBarAction -= StartFillHeartCoroutine;
        }

        public void InitHeart(Core.Characters.PlayerStats stats)
        {
            if (stats == null)
                return;

            _playerStats = stats;

            // 체력은 5단위
            // 하트 하나는 20
            var maxHeartAmount = _playerStats.MaxHP / 20;
            // var currFullHeartAmount = _playerStats.CurrentHP / 20;
            var currFullHeartAmount = 70 / 20;                          // 테스트
            // var remainValue = _playerStats.CurrentHP % 20;
            var remainValue = 70 % 20;                                  // 테스트

            bool _emptyHeart = false;   // 빈 하트인지

            for (int index = 0; index < maxHeartAmount; index++)
            {
                var tempHeart = _heartPool.GetObject();

                if (tempHeart.TryGetComponent(out HeartObject heart))
                {
                    tempHeart.SetActive(true);
                    tempHeart.transform.SetAsLastSibling();

                    if (_emptyHeart)
                        heart.SetHeart(HeartObject.HeartType.Empty);
                    else
                    {
                        if (index < currFullHeartAmount)
                            heart.SetHeart(HeartObject.HeartType.Full);
                        else
                        {
                            heart.SetHeart(GetHeartType(remainValue));

                            _emptyHeart = true; // 이번 인덱스가 마지막 채워진 하트
                        }
                    }

                    _heartList.Add(heart);
                }
            }
        }

        private HeartObject.HeartType GetHeartType(int currHP)
        {
            var value = (float)currHP / 20;

            if (Mathf.Approximately(value, .25f))
                return HeartObject.HeartType.AQuarter;
            else if (Mathf.Approximately(value, .5f))
                return HeartObject.HeartType.Half;
            else if (Mathf.Approximately(value, .75f))
                return HeartObject.HeartType.ThreeQaurter;
            else if (Mathf.Approximately(value, 1f))
                return HeartObject.HeartType.Full;

            return HeartObject.HeartType.Empty;
        }

        private void StartFillHeartCoroutine(int value)
        {
            if (_fillHeartCoroutine != null)
            {
                StopCoroutine(_fillHeartCoroutine);
                _fillHeartCoroutine = null;
            }

            StartCoroutine(nameof(FillHeartCoroutine));
        }

        private System.Collections.IEnumerator FillHeartCoroutine(int value)
        {
            yield return null;
        }
    }
}
