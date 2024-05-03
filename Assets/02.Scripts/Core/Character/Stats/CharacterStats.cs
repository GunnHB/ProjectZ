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

        public UnityEvent<int> OnHealthEvent = new();
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

            CurrentHP = (int)Mathf.Clamp(CurrentHP + amount, 0f, _maxHP);

            // 체력이 0이면 죽음
            if (CurrentHP == 0)
            {
                _isDeath = true;
                OnDeathEvent?.Invoke();

                return;
            }

            OnHealthEvent?.Invoke(amount);
        }
    }
}