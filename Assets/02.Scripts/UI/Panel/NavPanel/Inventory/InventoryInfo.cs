using UnityEngine;
using UnityEngine.Events;

using TMPro;

using ProjectZ.Manager;

namespace ProjectZ.UI
{
    public class InventoryInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _itemDesc;

        private void OnEnable()
        {
            ItemManager.Instance.OnSlotEvent.AddListener(SetInfo);
        }

        private void OnDisable()
        {
            ItemManager.Instance.OnSlotEvent.AddListener(SetInfo);
        }

        private void SetInfo(Model.ModelItem.Data data)
        {
            if (data == null)
            {
                _itemName.text = string.Empty;
                _itemDesc.text = string.Empty;

                return;
            }

            _itemName.text = data.name;
            _itemDesc.text = data.desc;
        }
    }
}
