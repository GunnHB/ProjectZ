using UnityEngine;
using UnityEngine.Events;

using Sirenix.OdinInspector;

using TMPro;

using DG.Tweening;

namespace ProjectZ.UI
{
    public class UIMenuPanel : UIPanelBase
    {
        public enum ContentType
        {
            None = -1,
            Inventory,
            Setting,
        }

        private const string GROUP_TOP = "[Top]";
        private const string GROUP_CONTENT = "[Content]";

        [Space]

        [TabGroup(GROUP_TOP)]
        [SerializeField] private TextMeshProUGUI _titleText;

        [TabGroup(GROUP_CONTENT)]
        [SerializeField] private Transform _contentTr;
        [TabGroup(GROUP_CONTENT)]
        [SerializeField] private UIInventoryNavPanel _inventoryNavPanel;

        private System.Collections.Generic.Dictionary<ContentType, UINavPanelBase> _contentDic = new();

        public UnityAction<ContentType> OnOpenAction;

        protected override void Init()
        {
            base.Init();

            InitContent();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            OnOpenAction += OnOpenCallback;
        }

        protected override void OnDisable()
        {
            OnOpenAction -= OnOpenCallback;
        }

        private void InitContent()
        {
            _contentDic.Add(ContentType.Inventory, _inventoryNavPanel);
        }

        private void OnOpenCallback(ContentType type)
        {
            if (_contentDic == null || !_contentDic.ContainsKey(type) || _contentDic[type] == null)
                return;

            var content = Instantiate(_contentDic[type]);

            if (content == null)
                return;

            content.transform.SetParent(_contentTr, false);

            _titleText.text = type.ToString().ToUpper();
        }

        /// <summary>
        /// MenuPanel이 열리고 닫힐 때 실행되는 트윈
        /// </summary>
        /// <param name="show">열지 닫을지</param>
        /// <param name="completeListener">트윈이 끝난 후 호출시킬 콜백</param>
        /// <returns></returns>
        public Sequence MenuTweenSequence(bool show, UnityAction completeListener = null)
        {
            var doEase = show ? Ease.InQuad : Ease.OutQuad;
            float targetScale = show ? 1f : 1.1f;
            float fromScale = show ? 1.1f : 1f;
            float targetAlpha = show ? 1f : 0f;
            float fromAlpha = show ? 0f : 1f;

            float duration = .2f;

            return DOTween.Sequence()
                        .OnStart(() =>
                        {
                            // 닫을 때만 실행됨
                            if (!show)
                            {
                                var popup = Manager.UIManager.Instance.GetUI<UIItemOptionPopup>();

                                if (popup != null)
                                    Manager.UIManager.Instance.CloseUI(popup);
                            }

                        })
                        .Append(transform.DOScale(targetScale, duration).From(fromScale).SetEase(doEase))
                        .Join(transform.GetComponent<CanvasGroup>().DOFade(targetAlpha, duration).From(fromAlpha))
                        .OnComplete(() =>
                        {
                            if (completeListener != null)
                                completeListener?.Invoke();

                            if (!show)
                                Manager.UIManager.Instance.CloseUI(this);
                        });
        }
    }
}
