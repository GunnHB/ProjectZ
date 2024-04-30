namespace ProjectZ.Core.FSM
{
    public class SprintState : BaseState
    {
        public SprintState(Characters.CharacterControls controls) : base(controls)
        {
        }

        public override void OperateEnter()
        {
            base.OperateEnter();

            // 속도 세팅
            _pControls?.DoSetMovementSpeed(Characters.MovementType.Sprint);
        }

        public override void OperateUpdate()
        {
            base.OperateUpdate();

            // NormalState와 동일한 로직
            _currVertical = 2f;

            // 이동 입력 없으면 normal 상태로 돌아감
            if (_pControls.MoveAmount < .1f)
                _pControls.NormalStateAction?.Invoke();

            SetFloat(_controls.ThisAnimData.AnimParamVerticalValue, _currVertical);
            UpdateStamina(Manager.GameValue.STAMINA_REDUCE);
        }

        public override void OperateExit()
        {
            base.OperateExit();

            // 속도 세팅
            if (!_pControls.ThisPlayerStats.IsExhauseted)
                _pControls?.DoSetMovementSpeed(Characters.MovementType.Normal);
        }
    }
}
