namespace ProjectZ.Core.FSM
{
    public class JumpState : BaseState
    {
        // 점프 시작 확인 플래그
        bool _jumpStarted = false;

        public JumpState(Characters.CharacterControls controls) : base(controls)
        {
        }

        public override void OperateEnter()
        {
            base.OperateEnter();

            _pControls.DoJump();
            _pControls.PerformingActionFlag = true;

            PlayAnimation(_pControls.ThisAnimData.AnimNameJump, 0f);
        }

        public override void OperateUpdate()
        {
            base.OperateUpdate();

            // 상태 진입 후 발이 땅에서 떨어지면 점프한 것으로 간주
            if (!_pControls.CheckGround())
                _jumpStarted = true;

            if (_pControls.CheckPeak())
                _pControls.FallingStateAction?.Invoke();
            else if (_jumpStarted && _pControls.CheckGround())
                _pControls.LandingStateAction?.Invoke();
        }

        public override void OperateExit()
        {
            base.OperateExit();

            _jumpStarted = false;
        }
    }
}
