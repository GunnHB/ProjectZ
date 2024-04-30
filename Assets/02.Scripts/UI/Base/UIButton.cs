using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using TMPro;

namespace ProjectZ.UI
{
    [RequireComponent(typeof(Image))]
    public class UIButton : Button, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        public enum EventType
        {
            None = -1,
            RightClick,
            Enter,
            Exit,
            Down,
            Up,
        }

        private Image _buttonImage;
        public Image ButtonImage
        {
            get
            {
                if (_buttonImage == null)
                    _buttonImage = GetComponent<Image>();

                return _buttonImage;
            }
        }

        private TextMeshProUGUI _buttonText;
        public TextMeshProUGUI ButtonText
        {
            get
            {
                if (_buttonText == null)
                    _buttonText = GetComponentInChildren<TextMeshProUGUI>();

                return _buttonText;
            }
        }

        private readonly System.Collections.Generic.Dictionary<EventType, UnityEvent> _eventDictionary = new();

        /// <summary>
        /// 버튼 일반 클릭 리스너 등록
        /// </summary>
        /// <param name="listener">버튼 콜백</param>
        /// <param name="doInit">기존 콜백 다 지울지</param>
        public void AddOnClickListener(UnityAction listener)
        {
            onClick.AddListener(listener);
        }

        /// <summary>
        /// 버튼 일반 클릭 리스너 제거
        /// </summary>
        /// <param name="listener"></param>
        public void RemoveOnClickListener(UnityAction listener)
        {
            onClick.RemoveListener(listener);
        }

        /// <summary>
        /// 버튼에 달린 모든 리스너 제거
        /// </summary>
        public void RemoveAllOnClickListeners()
        {
            onClick.RemoveAllListeners();
        }

        /// <summary>
        /// 버튼 이벤트 등록
        /// </summary>
        /// <param name="eventType">이벤트 타입</param>
        /// <param name="listener">콜백</param>
        public void Subscribe(EventType eventType, UnityAction listener)
        {
            if (_eventDictionary.TryGetValue(eventType, out UnityEvent thisEvent))
                thisEvent.AddListener(listener);
            else
            {
                thisEvent = new UnityEvent();

                thisEvent.AddListener(listener);
                _eventDictionary.Add(eventType, thisEvent);
            }
        }

        /// <summary>
        /// 버튼 이벤트 삭제
        /// </summary>
        /// <param name="eventType">이벤트 타입</param>
        /// <param name="listener">콜백</param>
        public void Unsubscribe(EventType eventType, UnityAction listener)
        {
            if (_eventDictionary.TryGetValue(eventType, out UnityEvent thisEvent))
                thisEvent.RemoveListener(listener);
        }

        /// <summary>
        /// 버튼 이미지 알파 값 세팅
        /// </summary>
        /// <param name="alpha">세팅 알파 값</param>
        public void SetImageAlpha(float alpha)
        {
            _buttonImage.color = new Color(_buttonImage.color.r, _buttonImage.color.g, _buttonImage.color.b, alpha);
        }

        /// <summary>
        /// 버튼 텍스트 세팅
        /// </summary>
        /// <param name="text"></param>
        public void SetButtonText(string text)
        {
            if (ButtonText != null)
                ButtonText.text = text;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            if (_eventDictionary.TryGetValue(EventType.Enter, out UnityEvent enterEvent))
                enterEvent?.Invoke();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            if (_eventDictionary.TryGetValue(EventType.Exit, out UnityEvent exitEvent))
                exitEvent?.Invoke();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            if (_eventDictionary.TryGetValue(EventType.Down, out UnityEvent downEvent))
                downEvent?.Invoke();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            if (_eventDictionary.TryGetValue(EventType.Up, out UnityEvent upEvent))
                upEvent?.Invoke();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (_eventDictionary.TryGetValue(EventType.RightClick, out UnityEvent rightClickEvent))
                    rightClickEvent?.Invoke();
            }
        }
    }
}
