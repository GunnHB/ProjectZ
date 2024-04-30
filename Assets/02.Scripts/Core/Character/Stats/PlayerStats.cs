using UnityEngine;

using Sirenix.OdinInspector;
using ProjectZ.UI;

namespace ProjectZ.Core.Characters
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "CharacterStats/New PlayerStats", order = int.MaxValue)]
    public class PlayerStats : CharacterStats
    {
        private const string TITLE_STAMINA = "[Stamina]";

        [Title(TITLE_STAMINA)]
        [SerializeField] protected float _maxStamina;

        public float MaxStamina => _maxStamina;
        public float CurrentStamina { get; protected set; }

        // delegate
        public delegate void OnExhausted();
        public event OnExhausted OnExhaustedEvent;

        // flags
        private bool _isExhausted = false;
        private bool _operateChargeStamina = false;

        // properties
        public bool IsExhauseted => _isExhausted;
        public bool OperateChargeStamina    // 스태미나 채울지 말지
        {
            get { return _operateChargeStamina; }
            set { _operateChargeStamina = value; }
        }

        public override void InitStats()
        {
            base.InitStats();
            CurrentStamina = _maxStamina;

            _operateChargeStamina = true;

            OpenPlayerHealthUI();
        }

        private void OpenPlayerHealthUI()
        {
            var health = Manager.UIManager.Instance.OpenUI<UIPlayerHealthBarHUD>();

            if (health == null)
                return;

            health.InitHeart(_maxHP);
        }

        public void SetStamina(float amount)
        {
            if (!_operateChargeStamina)
                return;

            CurrentStamina = Mathf.Clamp(CurrentStamina - amount, 0f, _maxStamina);

            if (!_isExhausted)
                _isExhausted = CurrentStamina == 0;

            if (_isExhausted)
            {
                if (CurrentStamina == _maxStamina)
                    _isExhausted = false;
            }

            if (CurrentStamina == 0)
                OnExhaustedEvent?.Invoke();
        }
    }
}
