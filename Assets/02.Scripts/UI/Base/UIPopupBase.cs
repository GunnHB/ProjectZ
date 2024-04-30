namespace ProjectZ.UI
{
    public class UIPopupBase : UIBase
    {
        protected const string TITLE_POPUP = "[Popup]";

        protected override void Init()
        {
            base.Init();

            _uiType = UIType.Popup;
        }
    }
}
