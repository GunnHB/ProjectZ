using UnityEngine;

namespace ProjectZ.Core.Characters
{
    public class EnemyControls : CharacterControls, IAttackable, IDamagable
    {
        // properties
        public EnemyStats ThisEnemyStats => _stats as EnemyStats;

        protected override void Awake()
        {
            base.Awake();

            InitAnimData();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            ThisStats.OnGetDamageEvent += OnGetDamageCallback;
            ThisStats.OnDeathEvent += OnDeathCallback;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            ThisStats.OnGetDamageEvent -= OnGetDamageCallback;
            ThisStats.OnDeathEvent -= OnDeathCallback;
        }

        protected override void Update()
        {
            base.Update();
        }

        private void InitAnimData()
        {
            _animData = new Data.EnemyAnimationData();

            _animData.InitializeData();
        }

        #region Attack
        public void InitAttackDatas()
        {
            throw new System.NotImplementedException();
        }

        public void AttackTarget(CharacterControls controls)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region Damage
        public void TakeDamage(float damageAmount)
        {
            ThisStats.SetHP((int)damageAmount);
        }

        private void OnGetDamageCallback()
        {
            _animator.applyRootMotion = true;
            _animator.CrossFade(_animData.AnimNameGetDamageFront, .1f);
        }

        private void OnDeathCallback()
        {
            _animator.applyRootMotion = true;
            _animator.CrossFade(_animData.AnimNameDeath, .1f);
        }
        #endregion
    }
}
