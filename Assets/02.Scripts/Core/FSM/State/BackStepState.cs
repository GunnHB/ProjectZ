namespace ProjectZ.Core.FSM
{
    public class BackStepState : BaseState
    {
        public BackStepState(Characters.CharacterControls controls) : base(controls)
        {
        }

        public override void OperateEnter()
        {
            base.OperateEnter();

            _pControls.CanMoveFlag = false;
            _pControls.PerformingActionFlag = true;

            UpdateStamina(Manager.GameValue.STAMINA_REDUCE_IMMEDIATE);
            PlayAnimation(_controls.ThisAnimData.AnimNameBackStep);
        }

        public override void OperateUpdate()
        {
            base.OperateUpdate();
        }

        public override void OperateExit()
        {
            base.OperateExit();
        }
    }
}
