namespace ProjectZ.Manager
{
    public class GameManager : SingletonObject<GameManager>
    {
        protected override void Awake()
        {
            base.Awake();

            InitializeAllModels();
        }

        private void InitializeAllModels()
        {
            Model.ModelItem.Model.Initialize();
            Model.ModelInventoryTab.Model.Initialize();
        }
    }
}
