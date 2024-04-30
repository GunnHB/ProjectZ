using UnityEngine;

using UnityEditor;

[CustomEditor(typeof(DetectDirection))]
public class DetectDirectionEditor : Editor
{
    private void OnSceneGUI()
    {
        DetectDirection detector = (DetectDirection)target;

        Handles.color = Color.white;
        Handles.DrawWireArc(detector.transform.position, Vector3.up, Vector3.forward, 360, detector.ThisRadius);
    }
}
