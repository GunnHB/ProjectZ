namespace ProjectZ.UI
{
    public class UINavPanelBase : UIPanelBase
    {
        // Awake 다음에 호출되도록
        protected override void Start()
        {
            base.Start();

            InitNavPanel();
        }

        protected virtual void InitNavPanel()
        {

        }
    }
}