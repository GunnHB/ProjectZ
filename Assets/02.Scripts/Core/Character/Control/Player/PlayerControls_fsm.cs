using UnityEngine.Events;

namespace ProjectZ.Core.Characters
{
    public partial class PlayerControls : CharacterControls
    {
        // components
        private FSM.StateMachine _stateMachine;

        // states
        private FSM.IState _normalState;
        private FSM.IState _rollState;
        private FSM.IState _backStepState;
        private FSM.IState _sprintState;
        private FSM.IState _jumpState;
        private FSM.IState _fallingState;
        private FSM.IState _landingState;
        private FSM.IState _attackState;

        // actions
        public UnityAction NormalStateAction;
        public UnityAction RollStateAction;
        public UnityAction BackStepStateAction;
        public UnityAction SprintStateAction;
        public UnityAction JumpStateAction;
        public UnityAction FallingStateAction;
        public UnityAction LandingStateAction;
        public UnityAction<Data.AttackData> AttackStateAction;

        private void InitStates()
        {
            _normalState = new FSM.NormalState(this);
            _rollState = new FSM.RollState(this);
            _backStepState = new FSM.BackStepState(this);
            _sprintState = new FSM.SprintState(this);
            _jumpState = new FSM.JumpState(this);
            _fallingState = new FSM.FallingState(this);
            _landingState = new FSM.LandingState(this);
            _attackState = new FSM.AttackState(this);

            _stateMachine = new FSM.StateMachine(_normalState);
        }

        private void RegistAllFSMActions()
        {
            NormalStateAction += NormalStateCallback;
            RollStateAction += RollStateCallback;
            BackStepStateAction += BackStepStateCallback;
            SprintStateAction += SprintStateCallback;
            JumpStateAction += JumpStateCallback;
            FallingStateAction += FallingStateCallback;
            LandingStateAction += LandingStateCallback;
            AttackStateAction += AttackStateCallback;
        }

        private void UnregistAllFSMActions()
        {
            NormalStateAction -= NormalStateCallback;
            RollStateAction -= RollStateCallback;
            BackStepStateAction -= BackStepStateCallback;
            SprintStateAction -= SprintStateCallback;
            JumpStateAction -= JumpStateCallback;
            FallingStateAction -= FallingStateCallback;
            LandingStateAction -= LandingStateCallback;
            AttackStateAction += AttackStateCallback;
        }

        #region NormalState
        private void NormalStateCallback()
        {
            _stateMachine.SwitchState(_normalState);
        }
        #endregion

        #region RollState
        private void RollStateCallback()
        {
            if (PerformingActionFlag || ThisPlayerStats.IsExhauseted)
                return;

            _stateMachine.SwitchState(_rollState);
        }
        #endregion

        #region BackStepState
        private void BackStepStateCallback()
        {
            if (PerformingActionFlag || ThisPlayerStats.IsExhauseted)
                return;

            _stateMachine.SwitchState(_backStepState);
        }
        #endregion

        #region SprintState
        private void SprintStateCallback()
        {
            if (ThisPlayerStats.IsExhauseted)
                return;

            _stateMachine.SwitchState(_sprintState);
        }
        #endregion

        #region JumpState
        private void JumpStateCallback()
        {
            if (PerformingActionFlag)
                return;

            if (!CheckGround())
                return;

            _stateMachine.SwitchState(_jumpState);
        }

        private void FallingStateCallback()
        {
            _stateMachine.SwitchState(_fallingState);
        }

        private void LandingStateCallback()
        {
            _stateMachine.SwitchState(_landingState);
        }
        #endregion

        #region AttackState
        private void AttackStateCallback(Data.AttackData attackData)
        {
            // // set attackdata
            (_attackState as FSM.AttackState).ThisAttackData = attackData;

            _stateMachine.SwitchState(_attackState, true);
        }
        #endregion
    }
}
