using UnityEngine;

using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

using UnityEditor;

public class SystemEditor : OdinEditorWindow
{
    private const string RUN = "Run";
    private const string HORIZONTALGROUP = "Run/Horizontal";

    [MenuItem("CustomEditor/SystemEditor")]
    public static void OpenWindow()
    {
        GetWindow<SystemEditor>();
    }

    [BoxGroup(RUN), Button(ButtonSizes.Gigantic), DisableIf(nameof(IsPlaying)), GUIColor("magenta")]
    public void Run()
    {
        EditorApplication.isPlaying = true;
    }

    [BoxGroup(RUN), HorizontalGroup(HORIZONTALGROUP), Button(ButtonSizes.Large)]
    [DisableIf(nameof(IsPlaying)), GUIColor("green")]
    public void BuildBundleAndRun()
    {
        CreateAssetBundle.BuildAssetBundles();

        EditorApplication.isPlaying = true;
    }

    [BoxGroup(RUN), HorizontalGroup(HORIZONTALGROUP), Button(ButtonSizes.Large)]
    [EnableIf(nameof(IsPlaying)), GUIColor("cyan")]
    public void Stop()
    {
        EditorApplication.isPlaying = false;
    }

    private bool IsPlaying()
    {
        return Application.isPlaying;
    }
}
