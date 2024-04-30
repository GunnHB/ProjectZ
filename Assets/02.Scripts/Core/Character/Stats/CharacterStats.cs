using UnityEngine;

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

        public delegate void OnDeath();
        public event OnDeath OnDeathEvent;

        public delegate void OnGetDamage();
        public event OnGetDamage OnGetDamageEvent;

        private bool _isDeath = false;

        public virtual void InitStats()
        {
            CurrentHP = _maxHP;

            _isDeath = false;
        }

        public void SetHP(int amount)
        {
            if (_isDeath)
                return;

            CurrentHP = (int)Mathf.Clamp(CurrentHP - amount, 0f, _maxHP);
            OnGetDamageEvent?.Invoke();

            // 체력이 0이면 죽음
            if (CurrentHP <= 0)
            {
                _isDeath = true;
                OnDeathEvent?.Invoke();
            }
        }
    }
}