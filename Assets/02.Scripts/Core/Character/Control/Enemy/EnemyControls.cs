using UnityEngine;

namespace ProjectZ.Core.Characters
{
    public partial class EnemyControls : CharacterControls
    {
        // properties
        public EnemyStats ThisEnemyStats => _stats as EnemyStats;

        [SerializeField] protected bool _isActiveIK = false;
        [SerializeField] protected Transform _targetObj;

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

        private void OnAnimatorIK()
        {
            if (_isActiveIK)
            {
                if (_targetObj != null)
                {
                    _animator.SetLookAtWeight(1);
                    _animator.SetLookAtPosition(_targetObj.position);
                }
            }
            else
                _animator.SetLookAtWeight(0);
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
