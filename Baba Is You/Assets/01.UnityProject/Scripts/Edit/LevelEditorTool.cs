#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelEditorTool : EditorWindow
{
    public static void Init_SetGridWindow()
    {
        SetGridWindow.Init();
    }
    public static void Init_SetStagePropertyWindow()
    {
        SetStagePropertyWindow.Init();
    }
    public static void Init_ObjectListWindow()
    {
        ObjectListWindow.Init();
    }

    [MenuItem("Window/Custom Tools/Level Editor/Enable")]
    public static void Enable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }
 
    [MenuItem("Window/Custom Tools/Level Editor/Disable")]
    public static void Disable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }
    private static void OnSceneGUI(SceneView sceneview)
    {
        Handles.BeginGUI();

        // 그리드 버튼
        if (GUILayout.Button("Set Grid", GUILayout.Width(100), GUILayout.Height(100)))
        {
            Init_SetGridWindow();
            Debug.Log("Button Clicked");
        }
        // 스테이지 정보 버튼
        if (GUILayout.Button("Stage Property", GUILayout.Width(100), GUILayout.Height(100)))
        {
            Init_SetStagePropertyWindow();
            Debug.Log("Button Clicked");
        }
        // 오브젝트 목록 버튼
        if (GUILayout.Button("Object List", GUILayout.Width(100), GUILayout.Height(100)))
        {
            Init_ObjectListWindow();
            Debug.Log("Button Clicked");
        }
        Handles.EndGUI();
    }
    
}

#endif