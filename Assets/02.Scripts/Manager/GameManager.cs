namespace ProjectZ.Manager
{
    public class GameManager : SingletonObject<GameManager>
    {
        public Core.Characters.PlayerStats ThisPlayerStats { get; set; }

        protected override void Awake()
        {
            base.Awake();

            InitializeAllModels();
        }

        private void InitializeAllModels()
        {
            Model.ModelItem.Model.Initialize();
            Model.ModelInventoryTab.Model.Initialize();
            Model.ModelFood.Model.Initialize();
        }
    }
}
