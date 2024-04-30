using UnityEngine;
using UnityEngine.Events;

using Sirenix.OdinInspector;

using TMPro;

using DG.Tweening;

using ProjectZ.Manager;

namespace ProjectZ.UI
{
    public class UICommonDialogPopup : UIPopupBase
    {
        private const string TITLE_DIALOG = "[Dialog]";

        [Title(TITLE_DIALOG)]
        [SerializeField] private RectTransform _popupGroup;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _descText;

        [SerializeField] private UIButton _cancelButton;
        [SerializeField] private UIButton _confirmButton;

        protected override void Init()
        {
            base.Init();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _confirmButton.AddOnClickListener(OnConfirmCallback);
            _cancelButton.AddOnClickListener(OnCancelCallback);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _confirmButton.RemoveOnClickListener(OnConfirmCallback);
            _cancelButton.RemoveOnClickListener(OnCancelCallback);
        }

        protected override void Start()
        {
            base.Start();

            PopupSequence(true);
        }

        public virtual void SetDialog(string title, string desc, string confirmString = "", string cancelString = "",
                                    UnityAction confirmListener = null, UnityAction cancelListener = null)
        {
            _titleText.text = title;
            _descText.text = desc;
            _confirmButton.SetButtonText(confirmString == string.Empty ? "Confrim" : confirmString);
            _cancelButton.SetButtonText(cancelString == string.Empty ? "Cancel" : cancelString);
        }

        private void OnConfirmCallback()
        {
            PopupSequence(false);
        }

        private void OnCancelCallback()
        {
            PopupSequence(false);
        }

        private Sequence PopupSequence(bool show)
        {
            float fromScale = show ? .9f : 1.1f;
            float targetScale = show ? 1.1f : .9f;

            float fromAlpha = show ? 0f : 1f;
            float targetAlpha = show ? 1f : 0f;

            return DOTween.Sequence()
                        .OnStart(() =>
                        {
                            _confirmButton.enabled = false;
                            _cancelButton.enabled = false;
                        })
                        .Append(_popupGroup.DOScale(targetScale, .3f).SetEase(Ease.OutBounce).From(fromScale))
                        .Join(_popupGroup.GetComponent<CanvasGroup>().DOFade(targetAlpha, .2f).From(fromAlpha))
                        .OnComplete(() =>
                        {
                            _confirmButton.enabled = true;
                            _cancelButton.enabled = true;
                        });
        }
    }
}
