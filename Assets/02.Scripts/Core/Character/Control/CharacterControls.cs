using System.Collections.Generic;

using UnityEngine;

using Sirenix.OdinInspector;

namespace ProjectZ.Core.Characters
{
    [RequireComponent(typeof(Movement))]
    public class CharacterControls : MonoBehaviour, IDamagable
    {
        private const string TITLE_COMMON = "[Common]";

        // components
        [Title(TITLE_COMMON)]
        [SerializeField] protected CharacterStats _stats;
        protected Movement _movement;
        protected Animator _animator;

        // datas
        protected Data.CharacterAnimationData _animData;

        // properties
        public Animator ThisAnimator => _animator;
        public Data.CharacterAnimationData ThisAnimData => _animData;
        public CharacterStats ThisStats => _stats;

        // collections
        protected List<Data.AttackData> _lightAttackDataList;
        protected List<Data.AttackData> _heavyAttackDataList;

        // flags
        protected bool _getHitted = false;

        protected virtual void Awake()
        {
            _movement = GetComponent<Movement>();
            _animator = GetComponent<Animator>();

            if (_stats != null)
                _stats.InitStats();
        }

        protected virtual void OnEnable()
        {
            _stats.OnHealthEvent.AddListener(PlayGetDamageAnimation);
            _stats.OnDeathEvent.RemoveListener(PlayDeathAnimation);
        }

        protected virtual void OnDisable()
        {
            _stats.OnHealthEvent.RemoveListener(PlayGetDamageAnimation);
            _stats.OnDeathEvent.RemoveListener(PlayDeathAnimation);
        }

        protected virtual void Update()
        {

        }

        public void DoSetMovementSpeed(MovementType type)
        {
            _movement.SetMovementSpeed(type);
        }

        public void DoJump()
        {
            _movement.JumpAction?.Invoke();
        }

        public bool CheckGround()
        {
            return _movement.GroundCheckFunc();
        }

        public bool CheckPeak()
        {
            return _movement.PeakCheckFunc();
        }

        public virtual void AnimEventHitCheakStart()
        {

        }

        public virtual void AnimEventHitCheakFinish()
        {

        }

        #region IDamagable
        public void TakeDamage(int damageAmount)
        {
            _stats.UpdateCurrentHP(damageAmount);
        }

        public void PlayGetDamageAnimation(int value)
        {
            if (value == 0 || Manager.TimeScaleManager.Instance.ThisTimeType == Manager.TimeScaleManager.TimeType.Pause)
                return;
            else if (value < 0)
            {
                _animator.applyRootMotion = true;
                _animator.CrossFade(_animData.AnimNameGetDamageFront, .1f);
            }
        }

        public void PlayDeathAnimation()
        {
            _animator.applyRootMotion = true;
            _animator.CrossFade(_animData.AnimNameDeath, .1f);
        }
        #endregion
    }
}
