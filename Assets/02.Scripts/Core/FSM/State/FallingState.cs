namespace ProjectZ.Core.FSM
{
    public class FallingState : BaseState
    {
        public FallingState(Characters.CharacterControls controls) : base(controls)
        {
        }

        public override void OperateEnter()
        {
            base.OperateEnter();

            _pControls.PerformingActionFlag = true;

            PlayAnimation(_pControls.ThisAnimData.AnimNameFalling, .1f);
        }

        public override void OperateUpdate()
        {
            base.OperateUpdate();

            if (_pControls.CheckGround())
                _pControls.LandingStateAction?.Invoke();
        }

        public override void OperateExit()
        {
            base.OperateExit();
        }
    }
}
