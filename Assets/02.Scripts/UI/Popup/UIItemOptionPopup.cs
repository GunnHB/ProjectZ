using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using ProjectZ.Data;

using Sirenix.OdinInspector;

using DG.Tweening;

namespace ProjectZ.UI
{
    public class UIItemOptionPopup : UIPopupBase
    {
        private const string TITLE_BUTTONS = "[Buttons]";
        private const string TITLE_COMPONENTS = "[Components]";

        private enum PositionType
        {
            None = -1,
            Up,
            Down,
        }

        private enum AnchorType
        {
            None = -1,
            Min,
            Max,
            Pivot,
        }

        [Title(TITLE_BUTTONS)]
        [SerializeField] private UIButton _useButton;
        [SerializeField] private UIButton _dropButton;
        [SerializeField] private UIButton _cancelButton;

        [Title(TITLE_COMPONENTS)]
        [SerializeField] private RectTransform _buttonGroup;
        [SerializeField] private InventorySlot _tempSlot;
        [SerializeField] private Image _bgImage;

        private Transform _targetSlotTransform;
        private InventoryItemData _invenItemData;
        private PositionType _positionType;

        private Vector3 _offset = new Vector3(0f, 60f, 0f);
        private Dictionary<PositionType, Dictionary<AnchorType, Vector2>> _posDictionary;

        private float _originBgAlpha = 0f;

        private bool _isEquipmentItem;

        protected override void Init()
        {
            base.Init();

            InitDictionary();
            InitAlpha();
        }

        private void InitDictionary()
        {
            _posDictionary = new()
            {
                {PositionType.Up, new()
                {
                    {AnchorType.Min, new Vector2(.5f, 1f)},
                    {AnchorType.Max, new Vector2(.5f, 1f)},
                    {AnchorType.Pivot, new Vector2(.5f, 1f)},
                }},
                {PositionType.Down, new()
                {
                    {AnchorType.Min, new Vector2(.5f, 0f)},
                    {AnchorType.Max, new Vector2(.5f, 0f)},
                    {AnchorType.Pivot, new Vector2(.5f, 0f)},
                }},
            };
        }

        private void InitAlpha()
        {
            _originBgAlpha = _bgImage.color.a;

            _bgImage.color = new Color(_bgImage.color.r, _bgImage.color.g, _bgImage.color.b, 0f);
            _buttonGroup.GetComponent<CanvasGroup>().alpha = 0f;
        }

        public void SetOptionPopup(InventoryItemData data, Transform slotTr, float scrollViewHight)
        {
            if (data == null)
                return;

            _invenItemData = data;
            _targetSlotTransform = slotTr;

            _isEquipmentItem = false;

            InitButtons();
            SetPosition(scrollViewHight);
            SetTempSlot(data);

            OpenSequence();
        }

        private void InitButtons()
        {
            _useButton.gameObject.SetActive(_invenItemData._inventoryItemData.type != Manager.GameValue.ItemType.Default);

            if (_invenItemData._isEquip)
            {
                _useButton.SetButtonText("REMOVE");
                _isEquipmentItem = true;
            }
            else
            {
                if (_invenItemData._inventoryItemData.type == Manager.GameValue.ItemType.Food)
                    _useButton.SetButtonText("USE");
                else
                {
                    _useButton.SetButtonText("EQUIP");
                    _isEquipmentItem = true;
                }
            }

            _dropButton.SetButtonText("DROP");
            _cancelButton.SetButtonText("CANCEL");
        }

        // ~~ 창 위치 조정 ~~
        private void SetPosition(float scrollViewHight)
        {
            if (_buttonGroup == null)
                return;

            _positionType = _targetSlotTransform.position.y > scrollViewHight ? PositionType.Up : PositionType.Down;
            SetAnchors(_positionType);
        }

        private void SetAnchors(PositionType type)
        {
            _buttonGroup.anchorMin = _posDictionary[type][AnchorType.Min];
            _buttonGroup.anchorMax = _posDictionary[type][AnchorType.Max];
            _buttonGroup.pivot = _posDictionary[type][AnchorType.Pivot];

            _buttonGroup.position = _targetSlotTransform.position - _offset * (type == PositionType.Up ? 1 : -1);
        }

        // // 선택된 슬롯으로 tempslot 세팅
        private void SetTempSlot(InventoryItemData data)
        {
            if (_tempSlot == null)
                return;

            _tempSlot.InitSlot(data);
            _tempSlot.OnUnregistAction?.Invoke();
            _tempSlot.transform.position = _targetSlotTransform.position;   // 아래의 선택된 슬롯의 위치로 세팅
        }

        protected override void OnEnable()
        {
            _useButton.AddOnClickListener(OnClickUseButton);
            _dropButton.AddOnClickListener(OnClickDropButton);
            _cancelButton.AddOnClickListener(OnClickCancelButton);
        }

        protected override void OnDisable()
        {
            _useButton.RemoveOnClickListener(OnClickUseButton);
            _dropButton.RemoveOnClickListener(OnClickDropButton);
            _cancelButton.RemoveOnClickListener(OnClickCancelButton);
        }

        private void OnClickUseButton()
        {
            if (_invenItemData._isEquip)
            {
                // 장착함
            }
            else
            {
                // 여기는 food
                if (!_isEquipmentItem)
                {
                    var foodItem = Model.ModelFood.Model.GetData(_invenItemData._inventoryItemData.ref_id);

                    if (foodItem == null)
                        return;

                    if (foodItem.target_stats == Manager.GameValue.StatsType.HP)
                        Manager.GameManager.Instance.ThisPlayerStats.OnHealthEvent?.Invoke((int)foodItem.increase_value);
                }
            }
        }

        private void OnClickDropButton()
        {
            if (_invenItemData._itemAmount > 1)
                Manager.UIManager.Instance.CloseUI(this, CloseSequence(() => OpenDialog(_invenItemData)));
            else
                Manager.UIManager.Instance.CloseUI(this, CloseSequence(() =>
                {
                    Manager.ItemManager.Instance.DropItem(_invenItemData);
                }));
        }

        private void OpenDialog(InventoryItemData itemData)
        {
            var dialog = Manager.UIManager.Instance.OpenUI<UISlideDialogPopup>();

            if (dialog != null)
                dialog.SetSlideDialog("Notice", "Select amount", itemData, string.Empty, string.Empty,
                                    () => Manager.ItemManager.Instance.DropItem(_invenItemData, dialog.SliderValue));
        }

        private void OnClickCancelButton()
        {
            Manager.UIManager.Instance.CloseUI(this, CloseSequence());
        }

        private Sequence OpenSequence()
        {
            return DOTween.Sequence()
                        .SetUpdate(true)
                        .OnStart(() =>
                        {
                            // 애니 시작 시 버튼 비활성화
                            _useButton.enabled = false;
                            _dropButton.enabled = false;
                            _cancelButton.enabled = false;
                        })
                        .Append(_bgImage.DOFade(_originBgAlpha, .2f).From(0f))
                        .Append(_buttonGroup.DOMoveY(_buttonGroup.position.y, .2f)
                                        .From(_buttonGroup.position.y + 15f * (_positionType == PositionType.Up ? 1 : -1)))
                        .Join(_buttonGroup.GetComponent<CanvasGroup>().DOFade(1f, .2f).From(0f))
                        .OnComplete(() =>
                        {
                            // 애니 완료 후 버튼 활성화
                            _useButton.enabled = true;
                            _dropButton.enabled = true;
                            _cancelButton.enabled = true;
                        });
        }

        private Sequence CloseSequence(UnityEngine.Events.UnityAction startListener = null)
        {
            return DOTween.Sequence()
                        .SetUpdate(true)
                        .OnStart(() =>
                        {
                            if (startListener != null)
                                startListener?.Invoke();

                            _useButton.enabled = false;
                            _dropButton.enabled = false;
                            _cancelButton.enabled = false;
                        })
                        .Append(_buttonGroup.DOMoveY(_buttonGroup.position.y + 15f * (_positionType == PositionType.Up ? 1 : -1f), .2f)
                                        .From(_buttonGroup.position.y))
                        .Join(_buttonGroup.GetComponent<CanvasGroup>().DOFade(0f, .2f).From(1f))
                        .Append(_bgImage.DOFade(0f, .3f).From(_originBgAlpha));
        }
    }
}
