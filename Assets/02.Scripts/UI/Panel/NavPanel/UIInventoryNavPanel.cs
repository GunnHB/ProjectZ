using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using ProjectZ.Manager;

using Sirenix.OdinInspector;

using DG.Tweening;
using System.Collections.Generic;

namespace ProjectZ.UI
{
    public class UIInventoryNavPanel : UINavPanelBase
    {
        private const string TITLE_LEFT = "[Left]";
        private const string TITLE_RIGHT = "[Right]";

        private const string GO_CAMERA_RAWIMAGE = "Camera_Inventory";

        [Title(TITLE_LEFT)]
        [SerializeField] private ObjectPool _tabPool;
        [SerializeField] private ObjectPool _rowPool;
        [SerializeField] private ScrollRect _scrollRect;

        [Title(TITLE_RIGHT)]
        [SerializeField] private RawImage _playerImage;
        [SerializeField] private InventoryInfo _invenInfo;

        private Vector3 _originInfoPosition;

        private bool _activeInfo = false;

        // for scroll rect
        private System.Collections.Generic.List<InventoryRow> _inventoryRowDic = new();
        private float _verticalSpacing;

        protected override void InitNavPanel()
        {
            base.InitNavPanel();

            InitTabs();
            InitInfo();
            InitRenderTexture();
        }

        protected override void OnEnable()
        {
            ItemManager.Instance.OnTabEvent.AddListener(InitSlots);
            ItemManager.Instance.OnSlotEvent.AddListener(InfoTweenCallback);
            ItemManager.Instance.OnScrollEvent.AddListener(OnScrollCallback);
        }

        protected override void OnDisable()
        {
            ItemManager.Instance.OnTabEvent.RemoveListener(InitSlots);
            ItemManager.Instance.OnSlotEvent.RemoveListener(InfoTweenCallback);
            ItemManager.Instance.OnScrollEvent.RemoveListener(OnScrollCallback);
        }

        private void InitTabs()
        {
            _tabPool.ReturnAllObject();

            var sortedData = Model.ModelInventoryTab.Model.DataList.OrderBy(x => x.order);

            foreach (var data in sortedData)
            {
                var tempObj = _tabPool.GetObject();

                if (tempObj.TryGetComponent(out InventoryTab tab))
                {
                    tab.Init(data);

                    if (data == sortedData.First())
                        ItemManager.Instance.CurrentInventoryTab = tab;
                }
            }
        }

        private void InitSlots(Model.ModelInventoryTab.Data tab)
        {
            _rowPool.ReturnAllObject();

            ItemManager.Instance.CurrentInventorySlot = null;   // 탭이 변경될 때 선택된 슬롯 정보도 초기화됨
            InfoTweenCallback(null);                                 // 탭 넘어갈 때 정보창 사라짐

            var slotList = ItemManager.Instance.ThisInventoryData.InventoryDic[tab.type];

            var rowAmount = slotList.Count / GameValue.INVEN_ROW_AMOUNT;         // 행의 개수
            var remainAmount = slotList.Count % GameValue.INVEN_ROW_AMOUNT;      // 슬롯의 나머지

            bool isRemain = remainAmount > 0;
            int maxIndex = isRemain ? rowAmount + 1 : rowAmount;

            for (int rowIndex = 0; rowIndex < maxIndex; rowIndex++)
            {
                var tempRow = _rowPool.GetObject();

                if (tempRow.TryGetComponent(out InventoryRow row))
                {
                    row.InitRow(rowIndex, isRemain && rowIndex == maxIndex - 1);

                    row.transform.SetAsLastSibling();
                }
            }

            SetScrollRect();
        }

        private void InitInfo()
        {
            if (_invenInfo.TryGetComponent(out CanvasGroup group))
                group.alpha = 0f;

            _activeInfo = false;
            _originInfoPosition = _invenInfo.transform.localPosition;
        }

        /// <summary>
        /// 정보창의 트윈
        /// </summary>
        /// <param name="data"></param>
        private void InfoTweenCallback(Model.ModelItem.Data data)
        {
            if (!_invenInfo.TryGetComponent(out CanvasGroup group))
                return;

            if (data == null)
            {
                if (_activeInfo)
                    InfoSequence(group, false);

                _activeInfo = false;
            }
            else
            {
                if (!_activeInfo)
                    InfoSequence(group, true);

                _activeInfo = true;
            }
        }

        private Sequence InfoSequence(CanvasGroup group, bool show)
        {
            float targetAlpha = show ? 1f : 0f;
            float fromAlpha = show ? 0f : 1f;
            float targetPositionX = show ? _originInfoPosition.x : _originInfoPosition.x + 10f;
            float fromPositionX = show ? _originInfoPosition.x + 10f : _originInfoPosition.x;

            float duration = .2f;

            return DOTween.Sequence()
                            .SetUpdate(true)
                            .Append(group.DOFade(targetAlpha, duration).From(fromAlpha))
                            .Join(group.transform.DOLocalMoveX(targetPositionX, duration).From(fromPositionX));
        }

        private void InitRenderTexture()
        {
            if (_playerImage == null)
                return;

            var cam = GameObject.Find(GO_CAMERA_RAWIMAGE).GetComponent<Camera>();
            var renderTexture = new RenderTexture(512, 512, 24);

            if (cam == null)
                return;

            _playerImage.texture = renderTexture;
            cam.targetTexture = renderTexture;

            if (!cam.gameObject.activeInHierarchy)
                cam.gameObject.SetActive(true);
        }

        private void SetScrollRect()
        {
            _inventoryRowDic.Clear();

            if (_scrollRect == null)
                return;

            foreach (var item in _scrollRect.content.GetComponentsInChildren<InventoryRow>())
                _inventoryRowDic.Add(item);

            _verticalSpacing = _scrollRect.content.GetComponent<VerticalLayoutGroup>().spacing;
            _scrollRect.verticalNormalizedPosition = 1f;
        }

        /// <summary>
        /// 특정 슬롯을 기준으로 스크롤 이동
        /// </summary>
        /// <param name="rowRect"></param> <summary>
        private void OnScrollCallback(InventoryRow row, UnityAction<float> listener = null)
        {
            if (_scrollRect == null || _inventoryRowDic.Count == 0)
                return;

            float posY = 0f;

            for (int index = 0; index < _inventoryRowDic.Count; index++)
            {
                if (row == _inventoryRowDic[index])
                    break;

                posY += (_inventoryRowDic[index].transform as RectTransform).rect.height;
                posY += _verticalSpacing;
            }

            var wholeHight = _scrollRect.content.rect.height - (_scrollRect.transform as RectTransform).rect.height;
            var targetPosition = 1 - (posY == 0 ? 0 : (posY / wholeHight < 1f ? posY / wholeHight : 1f));

            ScrollRectSequence(targetPosition, listener);
        }

        private Sequence ScrollRectSequence(float targetPosition, UnityAction<float> listener)
        {
            return DOTween.Sequence()
                        .SetUpdate(true)
                        .Append(DOTween.To(() => _scrollRect.verticalNormalizedPosition, x => _scrollRect.verticalNormalizedPosition = x, targetPosition, .2f))
                        .OnComplete(() =>
                        {
                            if (listener != null)
                                listener?.Invoke((_scrollRect.transform as RectTransform).rect.height);
                        });
        }
    }
}