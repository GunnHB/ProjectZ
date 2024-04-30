namespace ProjectZ.Core.FSM
{
    public class NormalState : BaseState
    {
        public NormalState(Characters.CharacterControls controls) : base(controls)
        {
        }

        public override void OperateEnter()
        {
            base.OperateEnter();
        }

        public override void OperateUpdate()
        {
            base.OperateUpdate();

            // 일반 움직임에서는 vertical 값으로만 조정하면 됨
            _currVertical = (_controls as Characters.PlayerControls).MoveAmount;

            SetFloat(_controls.ThisAnimData.AnimParamVerticalValue, _currVertical);
            UpdateStamina(Manager.GameValue.STAMINA_CHARGE, false);
        }

        public override void OperateExit()
        {
            base.OperateExit();
        }
    }
}
