using System.IO;

using UnityEngine;

namespace ProjectZ.Manager
{
    public class AssetBundleManager : SingletonObject<AssetBundleManager>
    {
        private const string BUNDLE_UI = "uibundle";
        private const string BUNDLE_ATALS = "atlasbundle";

        // assetbundles
        private static AssetBundle _uiBundle;
        private static AssetBundle _atlasBundle;

        // properties
        public AssetBundle UIBundle => _uiBundle;
        public AssetBundle AtlasBundle => _atlasBundle;

        protected override void Awake()
        {
            base.Awake();

            InitAllBundles();
        }

        private void InitAllBundles()
        {
            InitBundle(BUNDLE_UI, ref _uiBundle);
            InitBundle(BUNDLE_ATALS, ref _atlasBundle);
        }

        private void InitBundle(string bundleName, ref AssetBundle bundle)
        {
            if (bundle != null)
                return;

            bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundleName));
        }

        private AssetBundle GetUIBundle()
        {
            if (_uiBundle != null)
                return _uiBundle;

            _uiBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, BUNDLE_UI));

            return _uiBundle;
        }
    }
}