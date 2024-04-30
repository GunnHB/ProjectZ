using ProjectZ.Core.Characters;

namespace ProjectZ.Core.FSM
{
    public class AttackState : BaseState
    {
        private AttackData _currentAttackData;
        private AttackData _nextAttackData;

        // properties
        public AttackData ThisAttackData
        {
            set
            {
                if (_currentAttackData == null)
                    _currentAttackData = value;
                else
                    _nextAttackData = value;
            }
        }

        public bool AbleToComboAttack { get; private set; }
        public bool ComboFlag { get; set; }
        public bool StartedAttackFlag { get; private set; }

        public AttackState(CharacterControls controls) : base(controls)
        {
        }

        public override void OperateEnter()
        {
            base.OperateEnter();

            _pControls.CanMoveFlag = false;

            RefreshAttackData();

            if (_currentAttackData != null)
                PlayAnimation(_currentAttackData.AttackAnimHash);
        }

        public override void OperateUpdate()
        {
            base.OperateUpdate();

            if (!StartedAttackFlag)
            {
                if (CheckAnimationTag(TAG_ATTACK))
                    StartedAttackFlag = true;
            }
            else
            {
                AbleToComboAttack = GetCurrentAnimNormalizedTime() < _currentAttackData.TransitionNormalizedTime;

                if (GetCurrentAnimNormalizedTime() > .8f && _nextAttackData != null)
                    _pControls.AttackStateAction?.Invoke(_nextAttackData);
            }
        }

        public override void OperateExit()
        {
            base.OperateExit();

            if (_pControls.IsLastAttackData || !ComboFlag)
                _pControls.ResetAttackIndexAction.Invoke();

            StartedAttackFlag = false;
            AbleToComboAttack = false;
            ComboFlag = false;
        }

        private void RefreshAttackData()
        {
            if (_nextAttackData != null)
            {
                _currentAttackData = _nextAttackData;
                _nextAttackData = null;
            }
        }
    }
}
