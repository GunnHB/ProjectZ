using UnityEngine;
using UnityEngine.UI;

using ProjectZ.Manager;

namespace ProjectZ.UI
{
    public class InventoryTab : MonoBehaviour
    {
        private const float FLOAT_ENTER = .8f;
        private const float FLOAT_EXIT = .5F;

        [SerializeField] private UIButton _normal;
        [SerializeField] private Image _select;

        // data
        private Model.ModelInventoryTab.Data _tabData;
        public Model.ModelInventoryTab.Data TabData => _tabData;

        // flag
        private bool _isInit = false;

        public void Init(Model.ModelInventoryTab.Data tabData)
        {
            if (tabData == null || _isInit)
                return;

            _tabData = tabData;

            _normal.ButtonImage.sprite = AtlasManager.Instance.InventoryAtlas.GetSprite(_tabData.normal_sprite);
            _select.sprite = AtlasManager.Instance.InventoryAtlas.GetSprite(_tabData.select_sprite);

            SetSelect(false);

            _isInit = true;
        }

        private void OnEnable()
        {
            _normal.AddOnClickListener(OnClickCallback);

            _normal.Subscribe(UIButton.EventType.Enter, OnEnterCallback);
            _normal.Subscribe(UIButton.EventType.Exit, OnExitCallback);
        }

        private void OnDisable()
        {
            _normal.RemoveOnClickListener(OnClickCallback);

            _normal.Unsubscribe(UIButton.EventType.Enter, OnEnterCallback);
            _normal.Unsubscribe(UIButton.EventType.Exit, OnExitCallback);
        }

        private void OnClickCallback()
        {
            ItemManager.Instance.CurrentInventoryTab = this;
        }

        private void OnEnterCallback()
        {
            if (ItemManager.Instance.CurrentInventoryTab == this)
                return;

            _normal.SetImageAlpha(FLOAT_ENTER);
        }

        private void OnExitCallback()
        {
            if (ItemManager.Instance.CurrentInventoryTab == this)
                return;

            _normal.SetImageAlpha(FLOAT_EXIT);
        }

        public void SetSelect(bool active)
        {
            _normal.gameObject.SetActive(!active);
            _select.gameObject.SetActive(active);

            if (active)
                ItemManager.Instance.OnTabEvent?.Invoke(_tabData);
            else
                _normal.SetImageAlpha(FLOAT_EXIT);
        }
    }
}
