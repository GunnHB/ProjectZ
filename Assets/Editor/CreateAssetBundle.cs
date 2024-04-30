using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundle
{
    [MenuItem("Assets/Build All Asset Bundles")]
    public static void BuildAssetBundles()
    {
        string _assetBundleDir = "Assets/StreamingAssets";

        if (!Directory.Exists(Application.streamingAssetsPath))
            Directory.CreateDirectory(_assetBundleDir);

        BuildPipeline.BuildAssetBundles(_assetBundleDir, BuildAssetBundleOptions.None,
            EditorUserBuildSettings.activeBuildTarget);
    }
}
