using System;
using System.Collections.Generic;
using System.Linq;

using ProjectZ.Manager;

namespace ProjectZ.Data
{
    public class InventoryItemData
    {
        public Model.ModelItem.Data _inventoryItemData;
        public bool _isEquip;
        public int _itemAmount;

        public InventoryItemData()
        {
        }

        public InventoryItemData(Model.ModelItem.Data data)
        {
            _inventoryItemData = data;
            _isEquip = false;
            _itemAmount = 1;
        }

        public void ClearData()
        {
            _inventoryItemData = null;
            _isEquip = false;
            _itemAmount = 0;
        }

        public bool IsEmpty => _inventoryItemData == null;
    }

    public class InventoryData
    {
        // 아이템 타입 / 타입에 따른 아이템 데이터 리스트
        private Dictionary<GameValue.ItemType, List<InventoryItemData>> _inventoryDic = new();

        public Dictionary<GameValue.ItemType, List<InventoryItemData>> InventoryDic => _inventoryDic;
        public int Gold { get; set; }

        public InventoryData()
        {
            // 아이템 데이터
            _inventoryDic.Add(GameValue.ItemType.Weapon, ItemDataList(GameValue.INVEN_WEAPON_SLOT_AMOUNT));
            _inventoryDic.Add(GameValue.ItemType.Shield, ItemDataList(GameValue.INVEN_SHIELD_SLOT_AMOUNT));
            _inventoryDic.Add(GameValue.ItemType.Bow, ItemDataList(GameValue.INVEN_BOW_SLOT_AMOUNT));
            _inventoryDic.Add(GameValue.ItemType.Armor, ItemDataList(GameValue.INVEN_ARMOR_SLOT_AMOUNT));
            _inventoryDic.Add(GameValue.ItemType.Food, ItemDataList(GameValue.INVEN_FOOD_SLOT_AMOUNT));
            _inventoryDic.Add(GameValue.ItemType.Default, ItemDataList(GameValue.INVEN_DEFAULT_SLOT_AMOUNT));

            // 골드 데이터
            Gold = 0;
        }

        // 공갈 데이터 리스트
        private List<InventoryItemData> ItemDataList(int amount)
        {
            var list = new List<InventoryItemData>();

            for (int index = 0; index < amount; index++)
                list.Add(new InventoryItemData());

            return list;
        }

        internal void UpdateItemAmount(InventoryItemData invenItemData, int amount)
        {
            var tempData = TargetInventoryItemData(invenItemData);

            if (tempData == null)
                return;

            tempData._itemAmount -= amount;

            if (tempData._itemAmount <= 0)
                tempData.ClearData();

            ItemManager.Instance.OnSlotRefreshEvent?.Invoke(invenItemData);
        }

        private InventoryItemData TargetInventoryItemData(InventoryItemData itemData)
        {
            if (_inventoryDic[itemData._inventoryItemData.type].Contains(itemData))
                return _inventoryDic[itemData._inventoryItemData.type].Where(x => x == itemData).FirstOrDefault();
            else
                return null;
        }
    }
}
