using UnityEngine;
using UnityEngine.UI;

using Sirenix.OdinInspector;

namespace ProjectZ.UI
{
    public class UIPlayerStaminaHUD : UIHUDBase
    {
        [Title(TITLE_HUD)]
        [SerializeField] private GameObject _parent;
        [SerializeField] private Image _redWheel;
        [SerializeField] private Image _greenWheel;

        private Core.Characters.PlayerStats _stats;

        protected override void Init()
        {
            base.Init();

            _parent.SetActive(false);
        }

        public void SetStats(Core.Characters.PlayerStats stats)
        {
            _stats = stats;
        }

        /// <summary>
        /// 스태미나 ui 갱신
        /// </summary>
        /// <param name="activeRedWheel">감소할 때만 redWheel이 활성화됨</param>
        public void UpdateStaminaUI(bool activeRedWheel)
        {
            if (_stats == null)
                return;

            _parent.SetActive(_stats.CurrentStamina < _stats.MaxStamina);
            _redWheel.gameObject.SetActive(activeRedWheel);

            _greenWheel.fillAmount = _stats.CurrentStamina > 0 ? _stats.CurrentStamina / _stats.MaxStamina : 0f;
            _redWheel.fillAmount = _greenWheel.fillAmount + .07f;

            if (_stats.IsExhauseted)
                _redWheel.gameObject.SetActive(false);
        }
    }
}
