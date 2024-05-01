using UnityEngine;
using UnityEngine.Events;

using Sirenix.OdinInspector;

namespace ProjectZ.Core.Characters
{
    public class CharacterStats : ScriptableObject
    {
        private const string TITLE_HP = "[HP]";

        [Title(TITLE_HP)]
        [SerializeField] protected int _maxHP;

        public int MaxHP => _maxHP;

        public int CurrentHP { get; protected set; }

        // UnityEvent로 수정
        // public delegate void OnDeath();
        // public event OnDeath OnDeathEvent;

        // public delegate void OnGetDamage();
        // public event OnGetDamage OnGetDamageEvent;

        // public UnityEvent OnHealEvent = new();
        // public UnityEvent OnGetDamageEvent = new();
        public UnityEvent<int> OnUpdateHPEvent = new();
        public UnityEvent OnDeathEvent = new();

        private bool _isDeath = false;

        public virtual void InitStats()
        {
            CurrentHP = _maxHP;

            _isDeath = false;
        }

        /// <summary>
        /// 현재 체력 값 갱신
        /// </summary>
        /// <param name="amount">증가 / 감소시킬 수치</param>
        public void UpdateCurrentHP(int amount)
        {
            if (_isDeath)
                return;

            CurrentHP = (int)Mathf.Clamp(CurrentHP - amount, 0f, _maxHP);

            // if (amount < 0)
            //     OnGetDamageEvent?.Invoke();
            // else
            //     OnHealEvent?.Invoke();
            OnUpdateHPEvent?.Invoke(amount);

            // 체력이 0이면 죽음
            if (CurrentHP <= 0)
            {
                _isDeath = true;
                OnDeathEvent?.Invoke();
            }
        }
    }
}