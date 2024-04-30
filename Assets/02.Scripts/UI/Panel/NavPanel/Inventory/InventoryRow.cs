using UnityEngine;

using ProjectZ.Manager;

namespace ProjectZ.UI
{
    public class InventoryRow : MonoBehaviour
    {
        [SerializeField] private ObjectPool _slotPool;

        public void InitRow(int index, bool doRemain)
        {
            _slotPool.ReturnAllObject();

            var currTab = ItemManager.Instance.CurrentInventoryTab.TabData;
            var currList = ItemManager.Instance.ThisInventoryData.InventoryDic[currTab.type];

            int startIndex = index * GameValue.INVEN_ROW_AMOUNT;
            int endIndex = doRemain ? currList.Count : startIndex + GameValue.INVEN_ROW_AMOUNT;

            for (int slotIndex = startIndex; slotIndex < endIndex; slotIndex++)
            {
                var tempSlot = _slotPool.GetObject();

                if (tempSlot.TryGetComponent(out InventorySlot slot))
                {
                    slot.InitSlot(currList[slotIndex]);

                    slot.transform.SetAsLastSibling();
                }
            }
        }
    }
}
