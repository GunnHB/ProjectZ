namespace ProjectZ.UI
{
    public class UIFloatingBase : UIBase
    {
        protected const string TITLE_FLOATING = "[Floating]";

        protected override void Init()
        {
            base.Init();

            _uiType = UIType.Floating;
        }
    }
}
