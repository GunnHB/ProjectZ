using System.Collections.Generic;

using UnityEngine;

using Sirenix.OdinInspector;

namespace ProjectZ.Core.Characters
{
    [RequireComponent(typeof(Movement))]
    public class CharacterControls : MonoBehaviour
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
        protected List<AttackData> _lightAttackDataList;
        protected List<AttackData> _heavyAttackDataList;

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

        }

        protected virtual void OnDisable()
        {

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
    }
}
