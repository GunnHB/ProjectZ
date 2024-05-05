using UnityEngine;

namespace ProjectZ.Manager
{
    public class TimeScaleManager : SingletonObject<TimeScaleManager>
    {
        public enum TimeType
        {
            None = -1,
            Pause,
            Play,
            Slow,
            Fast,
        }

        private static TimeType _timeType = TimeType.None;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            // 정속으로 설정
            SetTimeSalce(TimeType.Play);
        }

        public void SetTimeSalce(TimeType timeType)
        {
            _timeType = timeType;

            Time.timeScale = GetTimeValue(_timeType);
        }

        private float GetTimeValue(TimeType timeType)
        {
            switch (timeType)
            {
                case TimeType.Pause:
                    return GameValue.TIME_PAUSE;
                case TimeType.Play:
                    return GameValue.TIME_PLAY;
                case TimeType.Slow:
                    return GameValue.TIME_SLOW;
                case TimeType.Fast:
                    return GameValue.TIME_FAST;
                default:
                    return -1;
            }
        }
    }
}
