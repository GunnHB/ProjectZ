using UnityEngine;
using UnityEngine.Events;

using ProjectZ.UI;

using DG.Tweening;

namespace ProjectZ.Manager
{
    public class UIManager : SingletonObject<UIManager>
    {
        private const string CANVAS_HUD = "HUDCanvas";
        private const string CANVAS_FLOATING = "FloatingCanvas";
        private const string CANVAS_PANEL = "PanelCanvas";
        private const string CANVAS_POPUP = "PopupCanvas";
        private const string CANVAS_SAFE_AREA = "SafeArea";

        public Canvas HUDCanvas => GetCanvas(CANVAS_HUD);
        public Canvas FloatingCanvas => GetCanvas(CANVAS_FLOATING);
        public Canvas PanelCanvas => GetCanvas(CANVAS_PANEL);
        public Canvas PopupCanvas => GetCanvas(CANVAS_POPUP);

        protected override void Awake()
        {
            base.Awake();

            InitAllSafeArea();
        }

        private void InitAllSafeArea()
        {
            InitSafeArea(HUDCanvas);
            InitSafeArea(FloatingCanvas);
            InitSafeArea(PanelCanvas);
            InitSafeArea(PopupCanvas);
        }

        /// <summary>
        /// 특정 UI 열기
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T OpenUI<T>() where T : UIBase
        {
            var openedUI = GetUI<T>();

            if (openedUI == null)
                openedUI = LoadUI<T>();

            return openedUI;
        }

        /// <summary>
        /// 하이어라키에 올라간 특정 ui 찾기
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetUI<T>() where T : UIBase
        {
            var canvas = GetCanvas<T>();

            if (canvas == null)
            {
                Debug.Log("there is no canvas");
                return null;
            }

            // ui 오브젝트들은 safearea 아래에 위치함
            Transform safeArea = canvas.transform.Find(CANVAS_SAFE_AREA);

            if (safeArea == null)
            {
                Debug.Log("there is no safearea object");
                return null;
            }

            for (int index = 0; index < safeArea.childCount; index++)
            {
                var childObj = safeArea.GetChild(index);

                if (childObj.TryGetComponent(out T ui))
                    return ui;
                else
                    continue;
            }

            return null;
        }

        private T LoadUI<T>() where T : UIBase
        {
            var canvas = GetCanvas<T>();

            if (canvas == null)
            {
                Debug.Log("there is no canvas");
                return null;
            }

            // ui 오브젝트들은 safearea 아래에 위치함
            Transform safeArea = canvas.transform.Find(CANVAS_SAFE_AREA);

            if (safeArea == null)
            {
                Debug.Log("there is no safearea object");
                return null;
            }

            // 에셋번들에서 불러오기
            // 불러오는 에셋의 이름이 같아야 함둥
            var prefab = AssetBundleManager.Instance.UIBundle.LoadAsset<GameObject>(typeof(T).Name);

            if (prefab == null)
                return null;

            var ui = Instantiate(prefab).GetComponent<T>();

            if (ui == null)
                return null;

            // worldPositionStays: true -> 오브젝트의 현재 위치는 유지한다.
            // worldPositionStays: false -> 오브젝트의 위치와 크기도 부모에 맞추게 된다.
            ui.transform.SetParent(safeArea, false);

            return ui;
        }

        private Canvas GetCanvas<T>() where T : UIBase
        {
            var baseType = typeof(T).BaseType;
            Canvas resultCanvas = null;

            // 베이스 찾을 때까지 반복
            while (true)
            {
                if (baseType.Equals(typeof(MonoBehaviour)))
                    break;

                if (baseType.Equals(typeof(UIHUDBase)))
                {
                    resultCanvas = HUDCanvas;
                    break;
                }
                else if (baseType.Equals(typeof(UIFloatingBase)))
                {
                    resultCanvas = FloatingCanvas;
                    break;
                }
                else if (baseType.Equals(typeof(UIPanelBase)))
                {
                    resultCanvas = PanelCanvas;
                    break;
                }
                else if (baseType.Equals(typeof(UIPopupBase)))
                {
                    resultCanvas = PopupCanvas;
                    break;
                }
                else
                    baseType = baseType.BaseType;
            }

            return resultCanvas;
        }

        private void InitSafeArea(Canvas canvas)
        {
            RectTransform saRT = canvas.transform.Find(CANVAS_SAFE_AREA) as RectTransform;
            Rect saRect = Screen.safeArea;

            Vector2 minAnchor = saRect.min;
            Vector2 maxAnchor = saRect.max;

            if (saRT == null)
                return;

            minAnchor.x /= Screen.width;
            minAnchor.y /= Screen.height;

            maxAnchor.x /= Screen.width;
            maxAnchor.y /= Screen.height;

            saRT.anchorMin = minAnchor;
            saRT.anchorMax = maxAnchor;
        }

        private Canvas GetCanvas(string canvasName)
        {
            Canvas canvas = null;
            GameObject obj = GameObject.Find(canvasName);

            if (obj != null)
                canvas = obj.GetComponent<Canvas>();

            return canvas;
        }

        /// <summary>
        /// ui가 열렸는지 확인
        /// </summary>
        /// <returns></returns>
        public bool IsOpendUI<T>() where T : UIBase
        {
            var canvas = GetCanvas<T>();

            if (canvas == null)
            {
                Debug.Log("there is no canvas");
                return false;
            }

            // ui 오브젝트들은 safearea 아래에 위치함
            Transform safeArea = canvas.transform.Find(CANVAS_SAFE_AREA);

            if (safeArea == null)
            {
                Debug.Log("there is no safearea object");
                return false;
            }

            for (int index = 0; index < safeArea.childCount; index++)
            {
                var childObj = safeArea.GetChild(index);

                if (childObj.TryGetComponent(out T compo))
                    return true;
                else
                    continue;
            }

            return false;
        }

        /// <summary>
        /// ui 닫기
        /// </summary>
        /// <param name="ui">닫을 ui</param>
        /// <typeparam name="T">UIBase</typeparam>
        public void CloseUI<T>(T ui) where T : UIBase
        {
            ui.Close();
        }

        /// <summary>
        /// 콜백 실행 후 ui 닫기
        /// </summary>
        /// <param name="ui">닫을 ui</param>
        /// <param name="listener">ui 닫을 때 호출시킬 콜백</param>
        /// <typeparam name="T">UIBase</typeparam>
        public void CloseUI<T>(T ui, UnityAction listener) where T : UIBase
        {
            listener?.Invoke();

            CloseUI(ui);
        }

        /// <summary>
        /// 시퀀스 실행 후 ui 닫기
        /// </summary>
        /// <param name="ui">닫을 ui</param>
        /// <param name="sequence">실행할 시퀀스 (시퀀스 종료 시에는 항상 ui 닫음)</param>
        /// <typeparam name="T"></typeparam>
        public void CloseUI<T>(T ui, Sequence sequence) where T : UIBase
        {
            sequence.SetDelay(.05f)                     // 약간 텀 주기
                    .OnComplete(() => CloseUI(ui));     // ui 닫기
        }
    }
}
