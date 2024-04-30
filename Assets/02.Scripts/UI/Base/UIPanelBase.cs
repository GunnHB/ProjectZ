namespace ProjectZ.UI
{
    public class UIPanelBase : UIBase
    {
        protected const string TITLE_PANEL = "[Panel]";

        protected override void Init()
        {
            base.Init();

            _uiType = UIType.Panel;
        }
    }
}
