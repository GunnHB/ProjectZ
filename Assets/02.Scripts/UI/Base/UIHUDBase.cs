namespace ProjectZ.UI
{
    public class UIHUDBase : UIBase
    {
        protected const string TITLE_HUD = "[HUD]";

        protected override void Init()
        {
            base.Init();

            _uiType = UIType.HUD;
        }
    }
}
