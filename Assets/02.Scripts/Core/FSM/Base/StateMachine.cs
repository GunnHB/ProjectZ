namespace ProjectZ.Core.FSM
{
    public class StateMachine
    {
        private IState _currentState;
        private IState _prevState;

        public IState CurrentState => _currentState;

        public StateMachine(IState initState)
        {
            SwitchState(initState);
        }

        public void DoOperatorUpdate()
        {
            // 현재 상태 진행 중
            _currentState.OperateUpdate();
        }

        public void SwitchState(IState newState, bool force = false)
        {
            // 현 상태가 새로운 상태와 같으면 교체하지 않음
            if (!force && _currentState != null && newState == _currentState)
                return;

            // 현재의 상태를 종료
            if (_currentState != null)
                _currentState.OperateExit();

            // 현재의 상태를 이전 상태로 교체한 뒤, 현재 상태를 새로운 상태로 교체
            _prevState = _currentState;
            _currentState = newState;

            // 교체된 상태로 진입
            _currentState.OperateEnter();
        }
    }
}
