using System.Linq;

using UnityEngine;
using UnityEngine.Events;

using ProjectZ.UI;
using ProjectZ.Data;

namespace ProjectZ.Manager
{
    public class ItemManager : SingletonObject<ItemManager>
    {
        // inventory data
        private InventoryData _inventoryData;
        public InventoryData ThisInventoryData => _inventoryData;

        // inventory tab
        private InventoryTab _currentInventoryTab;
        public InventoryTab CurrentInventoryTab
        {
            get => _currentInventoryTab;
            set
            {
                if (value == null)
                {
                    if (_currentInventoryTab != null)
                        _currentInventoryTab.SetSelect(false);

                    _currentInventoryTab = value;

                    return;
                }

                if (_currentInventoryTab != null)
                {
                    if (_currentInventoryTab == value)
                        return;

                    _currentInventoryTab.SetSelect(false);
                }

                _currentInventoryTab = value;
                _currentInventoryTab.SetSelect(true);
            }
        }

        // inventory slot
        private InventorySlot _currentInventorySlot;
        public InventorySlot CurrentInventorySlot
        {
            get => _currentInventorySlot;
            set
            {
                if (value == null)
                {
                    if (_currentInventorySlot != null)
                        _currentInventorySlot.SetSelect(false);

                    _currentInventorySlot = value;

                    return;
                }

                if (_currentInventorySlot != null)
                {
                    _currentInventorySlot.SetSelect(false);

                    if (_currentInventorySlot == value)
                    {
                        _currentInventorySlot = null;
                        OnSlotEvent?.Invoke(null);        // 이미 선택된 슬롯이라면 정보창 끔

                        return;
                    }
                }

                _currentInventorySlot = value;
                _currentInventorySlot.SetSelect(true);
            }
        }

        // events;
        public UnityEvent<Model.ModelInventoryTab.Data> OnTabEvent = new();
        public UnityEvent<Model.ModelItem.Data> OnSlotEvent = new();
        public UnityEvent<InventoryRow, UnityAction<float>> OnScrollEvent = new();

        public UnityEvent<InventoryItemData> OnSlotRefreshEvent = new();

        protected override void Awake()
        {
            base.Awake();

            InitInventory();
        }

        private void InitInventory()
        {
            // 가데이터임
            // 이후에는 외부에서 가져올 것
            _inventoryData = new InventoryData();

            AddItem(new InventoryItemData()
            {
                _inventoryItemData = Model.ModelItem.Model.DataList[0],
                _isEquip = true,
            });

            AddItem(new InventoryItemData()
            {
                _inventoryItemData = Model.ModelItem.Model.DataList.Last(),
                _itemAmount = 7,
            });

            AddItem(new InventoryItemData()
            {
                _inventoryItemData = Model.ModelItem.Model.DataList[7],
                _itemAmount = 15,
            });
        }

        public void AddItem(InventoryItemData invenItemData)
        {
            if (_inventoryData == null)
                return;

            // 인벤토리에 있는 아이템인지
            var exsitItem = _inventoryData.InventoryDic[invenItemData._inventoryItemData.type].Where(x =>
                            x._inventoryItemData == invenItemData._inventoryItemData).FirstOrDefault();

            if (exsitItem != null && exsitItem._inventoryItemData != null)
            {
                if (exsitItem._inventoryItemData.stackable)
                {
                    exsitItem._itemAmount += invenItemData._itemAmount;
                    return;
                }
            }

            for (int index = 0; index < _inventoryData.InventoryDic[invenItemData._inventoryItemData.type].Count; index++)
            {
                var slotData = _inventoryData.InventoryDic[invenItemData._inventoryItemData.type][index];

                // 빈 슬롯에 추가하기
                if (slotData._inventoryItemData == null || slotData._inventoryItemData.id == 0)
                {
                    _inventoryData.InventoryDic[invenItemData._inventoryItemData.type][index] = invenItemData;
                    break;
                }
            }
        }

        public void DropItem(InventoryItemData invenItemData, int amount = 1)
        {
            if (invenItemData == null)
                return;

            _inventoryData.DropItem(invenItemData, amount);
        }
    }
}
