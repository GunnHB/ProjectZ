using System;

using UnityEngine;
using UnityEngine.InputSystem;

using ProjectZ.UI;

namespace ProjectZ.Core.Characters
{
    public partial class PlayerControls : CharacterControls
    {
        // components
        private PlayerInputs _inputs;

        // properties
        public bool IsMoving => _moveAmount > .1f;
        public bool CanSprint => _moveAmount > .5f;
        public bool IsLastAttackData { get; private set; }

        private int _attackIndex;

        // action
        public Action ResetAttackIndexAction;
        public Action ResetMovementValuesAction;

        private void InitInputs()
        {
            _inputs = new PlayerInputs();
        }

        private void RegistAllInputs()
        {
            // movement
            RegistInputs(_inputs.PlayerMovement.Movement, null, PerformedMovementInput, null);

            // action
            RegistInputs(_inputs.PlayerActions.Dodge, null, PerformedDadgeInput, null);
            RegistInputs(_inputs.PlayerActions.Sprint, null, PerformedSprintInput, CanceledSprintInput);
            RegistInputs(_inputs.PlayerActions.Jump, null, PerformedJumpInput, null);
            RegistInputs(_inputs.PlayerActions.LightAttack, null, PerformedLightAttackInput, null);
            RegistInputs(_inputs.PlayerActions.HeavyAttack, null, PerformedHeavyAttackInput, null);

            // ui
            RegistInputs(_inputs.PlayerUI.Inventory, StartedInventoryInput);

            _inputs.Enable();
        }

        private void UnregistAllInputs()
        {
            // movement
            UnregistInputs(_inputs.PlayerMovement.Movement, null, PerformedMovementInput, null);

            // action
            UnregistInputs(_inputs.PlayerActions.Dodge, null, PerformedDadgeInput, null);
            UnregistInputs(_inputs.PlayerActions.Sprint, null, PerformedSprintInput, CanceledSprintInput);
            UnregistInputs(_inputs.PlayerActions.Jump, null, PerformedJumpInput, null);
            UnregistInputs(_inputs.PlayerActions.LightAttack, null, PerformedLightAttackInput, null);
            UnregistInputs(_inputs.PlayerActions.HeavyAttack, null, PerformedHeavyAttackInput, null);

            // ui
            UnregistInputs(_inputs.PlayerUI.Inventory, StartedInventoryInput);

            _inputs.Disable();
        }

        private void RegistInputs(InputAction inputAction,
                                Action<InputAction.CallbackContext> startedCallback = null,
                                Action<InputAction.CallbackContext> performedCallback = null,
                                Action<InputAction.CallbackContext> canceledCallback = null)
        {
            if (startedCallback != null)
                inputAction.started += startedCallback;

            if (performedCallback != null)
                inputAction.performed += performedCallback;

            if (canceledCallback != null)
                inputAction.canceled += canceledCallback;
        }

        private void UnregistInputs(InputAction inputAction,
                                Action<InputAction.CallbackContext> startedCallback = null,
                                Action<InputAction.CallbackContext> performedCallback = null,
                                Action<InputAction.CallbackContext> canceledCallback = null)
        {

            if (startedCallback != null)
                inputAction.started -= startedCallback;

            if (performedCallback != null)
                inputAction.performed -= performedCallback;

            if (canceledCallback != null)
                inputAction.canceled -= canceledCallback;
        }

        public void SetActiveMovement(bool active)
        {
            if (active)
            {
                _inputs.PlayerMovement.Enable();
                _inputs.PlayerActions.Enable();
                _inputs.PlayerCamera.Enable();

                _playerCam.enabled = true;
            }
            else
            {
                _inputs.PlayerMovement.Disable();
                _inputs.PlayerActions.Disable();
                _inputs.PlayerCamera.Disable();

                _playerCam.enabled = false;
            }
        }

        #region Movement
        private void PerformedMovementInput(InputAction.CallbackContext context)
        {
            if (!CanMoveFlag)
            {
                ResetMovementValuesAction?.Invoke();
                return;
            }

            _vertical = context.ReadValue<Vector2>().y;
            _horizontal = context.ReadValue<Vector2>().x;

            _moveDirection.x = _horizontal;
            _moveDirection.z = _vertical;

            _moveAmount = _moveDirection.magnitude;
        }

        private void ResetMovementValuesCallback()
        {
            _moveDirection = Vector3.zero;
            _moveAmount = 0f;
        }
        #endregion

        #region Dodge
        private void PerformedDadgeInput(InputAction.CallbackContext context)
        {
            // tap
            if (IsMoving)
                RollStateAction?.Invoke();  // do rolling
            else
                BackStepStateAction?.Invoke();   // do back step

        }
        #endregion

        #region Sprint
        private void PerformedSprintInput(InputAction.CallbackContext context)
        {
            // hold
            if (!CanSprint)
                return;

            SprintStateAction?.Invoke();
        }

        private void CanceledSprintInput(InputAction.CallbackContext context)
        {
            // release
            CancelSprint();
        }

        private void CancelSprint()
        {
            if (_stateMachine.CurrentState != _sprintState)
                return;

            NormalStateAction?.Invoke();
        }
        #endregion

        #region Jump
        private void PerformedJumpInput(InputAction.CallbackContext context)
        {
            JumpStateAction?.Invoke();
        }
        #endregion

        #region Attack
        private void PerformedLightAttackInput(InputAction.CallbackContext context)
        {
            // tap

            // 데이터 등록 확인
            if (_lightAttackDataList == null || _lightAttackDataList.Count == 0)
                return;

            IsLastAttackData = _attackIndex == _lightAttackDataList.Count;

            // AttackState에 진입 했지만 공격이 시작되지 않았으면 리턴
            if (_stateMachine.CurrentState == _attackState && !(_attackState as FSM.AttackState).StartedAttackFlag)
                return;
            // 후속 공격 입력 감지됐으면 리턴
            if ((_attackState as FSM.AttackState).ComboFlag)
                return;
            // 리스트의 마지막 공격이면 리턴
            if (IsLastAttackData)
                return;

            if ((_attackState as FSM.AttackState).AbleToComboAttack)
            {
                (_attackState as FSM.AttackState).ComboFlag = true;
                (_attackState as FSM.AttackState).ThisAttackData = _lightAttackDataList[_attackIndex];
            }
            else
                AttackStateAction?.Invoke(_lightAttackDataList[_attackIndex]);

            _attackIndex++;
        }

        private void PerformedHeavyAttackInput(InputAction.CallbackContext context)
        {
            // tap
            // Debug.Log("heavy");
        }
        #endregion

        #region Inventory
        private void StartedInventoryInput(InputAction.CallbackContext context)
        {
            var menu = Manager.UIManager.Instance.GetUI<UIMenuPanel>();

            if (menu == null)
            {
                menu = Manager.UIManager.Instance.OpenUI<UIMenuPanel>();

                if (menu != null)
                {
                    menu.MenuTweenSequence(true, () =>
                    {
                        menu.OnOpenAction?.Invoke(UIMenuPanel.ContentType.Inventory);
                        SetActiveMovement(false);
                    });
                }
            }
            else
                menu.MenuTweenSequence(false, () =>
                {
                    Manager.UIManager.Instance.CloseUI(menu, () => SetActiveMovement(true));
                });
        }
        #endregion
    }
}
