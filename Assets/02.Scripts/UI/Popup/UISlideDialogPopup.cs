using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using Sirenix.OdinInspector;

using TMPro;

using ProjectZ.Data;

using DG.Tweening;
using ProjectZ.Manager;

namespace ProjectZ.UI
{
    public class UISlideDialogPopup : UIPopupBase
    {
        private const string TITLE_TEXT = "[Text]";
        private const string TITLE_SLIDE = "[Slide]";
        private const string TITLE_BUTTON = "[Button]";
        private const string TITLE_GROUP = "[Group]";

        [Title(TITLE_GROUP)]
        [SerializeField] private Transform _popupGroup;

        [Title(TITLE_TEXT)]
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _descText;

        [Title(TITLE_SLIDE)]
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private UIButton _minusButton;
        [SerializeField] private UIButton _plusButton;

        [Title(TITLE_BUTTON)]
        [SerializeField] private UIButton _cancelButton;
        [SerializeField] private UIButton _confirmButton;

        private InventoryItemData _itemData;

        private UnityEvent OnConfirmEvent = new();
        private UnityEvent OnCancelEvent = new();

        public int SliderValue => (int)_slider.value;

        protected override void Init()
        {
            base.Init();

            PopupSequence(true);
        }

        public void SetSlideDialog(string title, string desc, InventoryItemData itemData, string confirmString, string cancelString,
                            UnityAction confirmListener = null, UnityAction cancelListener = null)
        {
            _titleText.text = title;
            _descText.text = desc;
            _confirmButton.SetButtonText(confirmString == string.Empty ? "Confrim" : confirmString);
            _cancelButton.SetButtonText(cancelString == string.Empty ? "Cancel" : cancelString);

            _itemData = itemData;

            _slider.maxValue = _itemData._itemAmount;
            _slider.value = 1;

            if (confirmListener != null)
                OnConfirmEvent.AddListener(confirmListener);

            if (cancelListener != null)
                OnCancelEvent.AddListener(cancelListener);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _slider.onValueChanged.AddListener(OnValueChangedCallback);

            _minusButton.AddOnClickListener(OnClickMinusButtonCallback);
            _plusButton.AddOnClickListener(OnClickPlusButtonCallback);

            _confirmButton.AddOnClickListener(OnConfirmCallback);
            _cancelButton.AddOnClickListener(OnCancelCallback);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _slider.onValueChanged.RemoveListener(OnValueChangedCallback);

            _minusButton.RemoveOnClickListener(OnClickMinusButtonCallback);
            _plusButton.RemoveOnClickListener(OnClickPlusButtonCallback);

            _confirmButton.RemoveOnClickListener(OnConfirmCallback);
            _cancelButton.RemoveOnClickListener(OnCancelCallback);
        }

        private void OnValueChangedCallback(float sliderValue)
        {
            _inputField.text = sliderValue.ToString();

            _minusButton.enabled = sliderValue > 0;
            _plusButton.enabled = sliderValue < _slider.maxValue;
        }

        private void OnClickMinusButtonCallback()
        {
            _slider.value -= 1;
        }

        private void OnClickPlusButtonCallback()
        {
            _slider.value += 1;
        }

        private void OnConfirmCallback()
        {
            if (OnConfirmEvent != null)
                OnConfirmEvent?.Invoke();

            UIManager.Instance.CloseUI(this, PopupSequence(false));
        }

        private void OnCancelCallback()
        {
            if (OnCancelEvent != null)
                OnCancelEvent?.Invoke();

            UIManager.Instance.CloseUI(this, PopupSequence(false));
        }

        private Sequence PopupSequence(bool show)
        {
            float fromScale = show ? .9f : 1.1f;
            float targetScale = show ? 1.1f : .9f;

            float fromAlpha = show ? 0f : 1f;
            float targetAlpha = show ? 1f : 0f;

            return DOTween.Sequence()
                        .SetUpdate(true)
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
