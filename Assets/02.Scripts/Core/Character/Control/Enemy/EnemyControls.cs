using UnityEngine;

namespace ProjectZ.Core.Characters
{
    public partial class EnemyControls : CharacterControls
    {
        // properties
        public EnemyStats ThisEnemyStats => _stats as EnemyStats;

        protected override void Awake()
        {
            base.Awake();

            InitAnimData();
            InitRootNodeList();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        private void Start()
        {
            InitBTRunner();
        }

        // 외부 클래스의 update문에서 호출
        public void DoUpdate()
        {
            OperateBTRunner();
        }

        private void InitAnimData()
        {
            _animData = new Data.EnemyAnimationData();

            _animData.InitializeData();
        }

        protected void PlayAnimation(int animHash)
        {

        }

        // #region Attack
        // public void InitAttackDatas()
        // {
        //     throw new System.NotImplementedException();
        // }

        // public void AttackTarget(CharacterControls controls)
        // {
        //     throw new System.NotImplementedException();
        // }
        // #endregion

        // #region Damage
        // public void TakeDamage(int damageAmount)
        // {
        //     ThisStats.UpdateCurrentHP((int)damageAmount);
        // }

        // public void OnUpdateHP(int value)
        // {
        //     _animator.applyRootMotion = true;
        //     _animator.CrossFade(_animData.AnimNameGetDamageFront, .1f);
        // }

        // private void OnGetDamageCallback()
        // {
        //     // _animator.applyRootMotion = true;
        //     // _animator.CrossFade(_animData.AnimNameGetDamageFront, .1f);
        // }

        // private void OnDeathCallback()
        // {
        //     _animator.applyRootMotion = true;
        //     _animator.CrossFade(_animData.AnimNameDeath, .1f);
        // }
        // #endregion
    }
}
