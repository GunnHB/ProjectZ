namespace ProjectZ.Core.FSM
{
    public class LandingState : BaseState
    {
        public LandingState(Characters.CharacterControls controls) : base(controls)
        {
        }

        public override void OperateEnter()
        {
            base.OperateEnter();

            PlayAnimation(_pControls.ThisAnimData.AnimNameLanding, .1f);
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
