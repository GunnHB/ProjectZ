using System;

using UnityEngine;

using TMPro;

using Sirenix.OdinInspector;

namespace ProjectZ.UI
{
    public class UIPlayerStateInfoHUD : UIHUDBase
    {
        [Title(TITLE_HUD)]
        [SerializeField] private TextMeshProUGUI _stateText;

        public Action<Core.FSM.IState> ChangeStateAction;

        protected override void Init()
        {
            base.Init();
        }

        protected override void OnEnable()
        {
            ChangeStateAction += ChangeStateCallback;
        }

        protected override void OnDisable()
        {
            ChangeStateAction -= ChangeStateCallback;
        }

        private void ChangeStateCallback(Core.FSM.IState newState)
        {
            _stateText.text = newState.ToString();
        }
    }
}
