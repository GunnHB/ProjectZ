using UnityEngine;

namespace ProjectZ.Manager
{
    public class SingletonCreator : MonoBehaviour
    {
        private void Awake()
        {
            InitAllSingletonObjects();
        }

        private void InitAllSingletonObjects()
        {
            InitSingletonObject<UIManager>();
            InitSingletonObject<AssetBundleManager>();
            InitSingletonObject<GameManager>();
            InitSingletonObject<AtlasManager>();
            InitSingletonObject<ItemManager>();
            InitSingletonObject<TimeScaleManager>();
        }

        private void InitSingletonObject<T>()
        {
            GameObject obj = new GameObject(typeof(T).Name);

            if (obj != null)
                obj.AddComponent(typeof(T));
        }
    }
}