using UnityEngine.U2D;

namespace ProjectZ.Manager
{
    public class AtlasManager : SingletonObject<AtlasManager>
    {
        private const string ATLAS_INVENTORY = "InventoryAtlas";
        private const string ATLAS_COMMON = "CommonAtlas";

        private SpriteAtlas _inventoryAtlas;
        public SpriteAtlas InventoryAtlas => _inventoryAtlas;

        private SpriteAtlas _commonAtlas;
        public SpriteAtlas CommonAtlas => _commonAtlas;

        protected override void Awake()
        {
            base.Awake();

            InitAllAtlas();
        }

        private void OnEnable()
        {
            SpriteAtlasManager.atlasRequested += ReqeustAtlas;
        }

        private void OnDisable()
        {
            SpriteAtlasManager.atlasRequested -= ReqeustAtlas;
        }

        private void ReqeustAtlas(string tag, System.Action<SpriteAtlas> callback)
        {
            var atlas = UnityEngine.Resources.Load<SpriteAtlas>(tag);

            callback(atlas);
        }

        private void InitAllAtlas()
        {
            InitAtlas(ATLAS_INVENTORY, ref _inventoryAtlas);
            InitAtlas(ATLAS_COMMON, ref _commonAtlas);
        }

        private void InitAtlas(string atlasName, ref SpriteAtlas atlas)
        {
            if (atlas != null)
                return;

            atlas = AssetBundleManager.Instance.AtlasBundle.LoadAsset<SpriteAtlas>(atlasName);
        }
    }
}