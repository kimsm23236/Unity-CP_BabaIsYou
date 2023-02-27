using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CustomTool : MonoBehaviour
{
    [MenuItem("Window/Custom Tools/Enable")]
    public static void Enable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }
 
    [MenuItem("Window/Custom Tools/Disable")]
    public static void Disable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }
 
    private static void OnSceneGUI(SceneView sceneview)
    {
        Handles.BeginGUI();
 
        if (GUILayout.Button("Button"))
            Debug.Log("Button Clicked");
 
        Handles.EndGUI();
    }
}
