using UnityEngine;

using Sirenix.OdinInspector;

namespace ProjectZ.UI
{
    public class UIBase : MonoBehaviour
    {
        private const string TITLE_COMMON = "[Common]";

        protected enum UIType
        {
            None = -1,
            HUD,
            Panel,
            Popup,
            Floating,
        }

        [Title(TITLE_COMMON)]
        [SerializeField] protected UIType _uiType;

        private void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {

        }

        protected virtual void Start()
        {

        }

        protected virtual void OnEnable()
        {

        }

        protected virtual void OnDisable()
        {

        }

        public virtual void Close()
        {
            Destroy(gameObject);
        }
    }
}