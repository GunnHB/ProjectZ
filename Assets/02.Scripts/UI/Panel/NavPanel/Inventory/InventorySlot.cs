using UnityEngine;
using UnityEngine.UI;

using TMPro;

using Sirenix.OdinInspector;

using ProjectZ.Manager;
using ProjectZ.Data;

namespace ProjectZ.UI
{
    public class InventorySlot : MonoBehaviour
    {
        private const string TITLE_IMAGE = "[Image]";
        private const string TITLE_TEXT = "[Text]";
        private const string TITLE_BUTTON = "[Button]";

        private const float FLOAT_ENTER = .65f;
        private const float FLOAT_EXIT = 0f;
        private const float FLOAT_SELECT = 1f;

        [Title(TITLE_IMAGE)]
        [SerializeField] private Image _frameImage;
        [SerializeField] private Image _itemImage;

        [Title(TITLE_TEXT)]
        [SerializeField] private TextMeshProUGUI _equipText;
        [SerializeField] private TextMeshProUGUI _amountText;

        [Title(TITLE_BUTTON)]
        [SerializeField] private UIButton _slotButton;

        private InventoryItemData _invenItemData;
        private Model.ModelItem.Data _itemData;

        // 외부에서 슬롯에 등록된 리스너를 해제하기 위한 액션
        public UnityEngine.Events.UnityAction OnUnregistAction;

        private bool _isSelected = false;

        // 인벤토리 데이터의 값으로 초기화해야함
        public void InitSlot(InventoryItemData invenItemData)
        {
            if (invenItemData == null)
                return;
            else if (invenItemData._inventoryItemData == null)
            {
                ClearData();
                return;
            }

            _invenItemData = invenItemData;
            _itemData = invenItemData._inventoryItemData;

            _itemImage.sprite = AtlasManager.Instance.InventoryAtlas.GetSprite(_itemData.sprite);
            _itemImage.gameObject.SetActive(true);
            _equipText.gameObject.SetActive(invenItemData._isEquip);

            _amountText.text = invenItemData._itemAmount.ToString();
            _amountText.gameObject.SetActive(_itemData.stackable);
        }

        private void OnEnable()
        {
            RegistListeners();

            OnUnregistAction += UnregistListeners;
        }

        private void OnDisable()
        {
            UnregistListeners();

            OnUnregistAction -= UnregistListeners;
        }

        private void RegistListeners()
        {
            _slotButton.AddOnClickListener(OnClickCallback);

            _slotButton.Subscribe(UIButton.EventType.Enter, OnEnterCallback);
            _slotButton.Subscribe(UIButton.EventType.Exit, OnExitCallback);
            _slotButton.Subscribe(UIButton.EventType.RightClick, OnRightClickCallback);
        }

        private void UnregistListeners()
        {
            _slotButton.RemoveOnClickListener(OnClickCallback);

            _slotButton.Unsubscribe(UIButton.EventType.Enter, OnEnterCallback);
            _slotButton.Unsubscribe(UIButton.EventType.Exit, OnExitCallback);
            _slotButton.Unsubscribe(UIButton.EventType.RightClick, OnRightClickCallback);
        }

        private void OnClickCallback()
        {
            ItemManager.Instance.CurrentInventorySlot = this;

            ItemManager.Instance.OnScrollEvent?.Invoke(transform.parent.GetComponent<InventoryRow>(), null);
        }

        private void OnEnterCallback()
        {
            if (ItemManager.Instance.CurrentInventorySlot == this)
                return;

            SetFrameAlpha(FLOAT_ENTER);
        }

        private void OnExitCallback()
        {
            if (ItemManager.Instance.CurrentInventorySlot == this)
                return;

            SetFrameAlpha(FLOAT_EXIT);
        }

        private void OnRightClickCallback()
        {
            // 선택안된 상태에서만 실행됨
            if (!_isSelected)
                ItemManager.Instance.CurrentInventorySlot = this;

            ItemManager.Instance.OnScrollEvent?.Invoke(transform.parent.GetComponent<InventoryRow>(), OpenOptionPopup);
        }

        private void OpenOptionPopup(float scollViewHight)
        {
            // 빈 슬롯은 옵션창을 띄울 필요가 엄서요
            if (_invenItemData == null)
                return;

            var optionPopup = UIManager.Instance.GetUI<UIItemOptionPopup>();

            if (optionPopup == null)
            {
                optionPopup = UIManager.Instance.OpenUI<UIItemOptionPopup>();

                if (optionPopup != null)
                    optionPopup.SetOptionPopup(_invenItemData, transform, scollViewHight);
            }
            else
                optionPopup.SetOptionPopup(_invenItemData, transform, scollViewHight);
        }

        private void SetFrameAlpha(float alpha)
        {
            if (!_frameImage.gameObject.activeInHierarchy)
                _frameImage.gameObject.SetActive(true);

            _frameImage.color = new Color(1f, 1f, 1f, alpha);
        }

        public void SetSelect(bool active)
        {
            _isSelected = active;
            _frameImage.gameObject.SetActive(active);

            if (active)
            {
                SetFrameAlpha(FLOAT_SELECT);
                ItemManager.Instance.OnSlotRefreshEvent.AddListener(RefreshSlot);
            }
            else
                ItemManager.Instance.OnSlotRefreshEvent.RemoveListener(RefreshSlot);

            ItemManager.Instance.OnSlotEvent?.Invoke(_itemData);
        }

        private void RefreshSlot(InventoryItemData invenItemData)
        {
            _invenItemData = invenItemData;

            if (_invenItemData._itemAmount <= 0)
            {
                ClearData();
                ItemManager.Instance.OnSlotEvent.Invoke(_itemData);

                return;
            }

            _amountText.text = _invenItemData._itemAmount.ToString();
            _equipText.gameObject.SetActive(_invenItemData._isEquip);
        }

        private void ClearData()
        {
            _invenItemData = null;
            _itemData = null;

            _itemImage.gameObject.SetActive(false);

            _equipText.gameObject.SetActive(false);
            _amountText.gameObject.SetActive(false);

            _frameImage.gameObject.SetActive(false);
        }
    }
}