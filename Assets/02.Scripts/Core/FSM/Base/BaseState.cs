using UnityEngine;

namespace ProjectZ.Core.FSM
{
    public class BaseState : IState
    {
        protected const string TAG_ATTACK = "Tag_Attack";

        protected Characters.CharacterControls _controls;
        protected Characters.PlayerControls _pControls;

        // 상태머신 확인용
        protected UI.UIPlayerStateInfoHUD _stateInfoHUD;

        // 스태미나 ui
        protected UI.UIPlayerStaminaInfoFloating _staminaInfo;

        protected float _currVertical = 0f;

        public BaseState(Characters.CharacterControls controls)
        {
            _controls = controls;
            _pControls = controls as Characters.PlayerControls;

            InitUIs();
        }

        private void InitUIs()
        {
            // 상태머신 확인용
            if (_stateInfoHUD == null)
                _stateInfoHUD = Manager.UIManager.Instance.OpenUI<UI.UIPlayerStateInfoHUD>();

            // 스태미나 ui
            if (_staminaInfo == null)
            {
                _staminaInfo = Manager.UIManager.Instance.OpenUI<UI.UIPlayerStaminaInfoFloating>();

                if (_staminaInfo != null)
                    _staminaInfo.SetStats(_pControls.ThisPlayerStats);
            }
        }

        public virtual void OperateEnter()
        {
            // Debug.Log($"{this} enter");

            // 상태머신 확인용
            if (_stateInfoHUD != null)
                _stateInfoHUD.ChangeStateAction?.Invoke(this);

            _currVertical = GetFloat(_controls.ThisAnimData.AnimParamVerticalValue);
        }

        public virtual void OperateUpdate()
        {
            // Debug.Log($"{this} update");
        }

        public virtual void OperateExit()
        {
            // Debug.Log($"{this} exit");
        }

        protected float GetFloat(int animHash)
        {
            return _controls.ThisAnimator.GetFloat(animHash);
        }

        protected void SetFloat(int animHash, float value)
        {
            _controls.ThisAnimator.SetFloat(animHash, value, .1f, Time.deltaTime);

            if (Mathf.Abs(GetFloat(animHash) - value) < .01f)
                _controls.ThisAnimator.SetFloat(animHash, value);
        }

        protected void PlayAnimation(int animHash, bool applyRootMotion = true)
        {
            _controls.ThisAnimator.applyRootMotion = applyRootMotion;
            _controls.ThisAnimator.CrossFade(animHash, .2f);
        }

        protected void PlayAnimation(int animHash, float durationTime)
        {
            _controls.ThisAnimator.CrossFade(animHash, durationTime);
        }

        /// <summary>
        /// 스태미나 갱신
        /// </summary>
        /// <param name="target">수치 값</param>
        /// <param name="reduce">감소? 증가?</param>
        protected void UpdateStamina(float target, bool reduce = true)
        {
            if (_staminaInfo == null)
                return;

            if (!reduce)
                target = -target;

            _pControls.ThisPlayerStats.SetStamina(target);
            _staminaInfo.UpdateStaminaUI(reduce);
        }

        protected bool CheckAnimationTag(string tag, int layerIndex = 1)
        {
            return _controls.ThisAnimator.GetCurrentAnimatorStateInfo(layerIndex).IsTag(tag);
        }

        protected float GetCurrentAnimNormalizedTime(int layerIndex = 1)
        {
            return _controls.ThisAnimator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime;
        }
    }
}
