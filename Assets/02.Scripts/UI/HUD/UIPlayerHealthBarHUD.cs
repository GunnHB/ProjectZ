using System.Collections;

using UnityEngine;
using UnityEngine.Events;

using ProjectZ.Manager;

using DG.Tweening;

namespace ProjectZ.UI
{
    public class UIPlayerHealthBarHUD : UIHUDBase
    {
        [SerializeField] private ObjectPool _heartPool;

        private Core.Characters.PlayerStats _playerStats;

        public UnityAction<int> OnHealthBarAction;

        private System.Collections.Generic.List<HeartObject> _heartList = new();
        private int _lastFillHeartIndex = 0;

        private Coroutine _healthBarCoroutine;

        protected override void Init()
        {
            base.Init();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            OnHealthBarAction += StartHealthBarCoroutine;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            OnHealthBarAction -= StartHealthBarCoroutine;
        }

        public void InitHeart(Core.Characters.PlayerStats stats)
        {
            if (stats == null)
                return;

            _playerStats = stats;

            // 체력은 5단위
            // 하트 하나는 20
            var maxHeartAmount = _playerStats.MaxHP / 20;
            var currFullHeartAmount = _playerStats.CurrentHP / 20;
            var remainValue = _playerStats.CurrentHP % 20;

            bool _emptyHeart = false;   // 빈 하트인지

            for (int index = 0; index < maxHeartAmount; index++)
            {
                var tempHeart = _heartPool.GetObject();

                if (tempHeart.TryGetComponent(out HeartObject heart))
                {
                    tempHeart.SetActive(true);
                    tempHeart.transform.SetAsLastSibling();

                    if (_emptyHeart)
                        heart.SetHeart(HeartObject.HeartType.None);
                    else
                    {
                        if (index < currFullHeartAmount)
                            heart.SetHeart(HeartObject.HeartType.Full);
                        else
                        {
                            heart.SetHeart(GetHeartType(remainValue));

                            _emptyHeart = true; // 이번 인덱스가 마지막 채워진 하트
                        }

                        _lastFillHeartIndex = index;
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
            else
                return HeartObject.HeartType.None;
        }

        private void StartHealthBarCoroutine(int value)
        {
            if (_healthBarCoroutine != null)
            {
                StopCoroutine(_healthBarCoroutine);
                _healthBarCoroutine = null;
            }

            _healthBarCoroutine = StartCoroutine(nameof(HealthBarCoroutine), value);
        }

        private IEnumerator HealthBarCoroutine(int value)
        {
            var tempValue = Mathf.Abs(value) / 5;

            for (int index = 0; index < tempValue; index++)
            {
                _heartList[_lastFillHeartIndex].ActiveHeartByOne(value > 0);

                if (value > 0)
                {
                    if (_heartList[_lastFillHeartIndex].IsFull)
                        _lastFillHeartIndex++;
                }
                else
                {
                    if (_heartList[_lastFillHeartIndex].IsEmpty)
                        _lastFillHeartIndex--;
                }

                _lastFillHeartIndex = Mathf.Clamp(_lastFillHeartIndex, 0, _heartList.Count - 1);

                // yield return new WaitForSeconds(.05f);
                yield return new WaitForSecondsRealtime(.05f);
            }
        }
    }
}
