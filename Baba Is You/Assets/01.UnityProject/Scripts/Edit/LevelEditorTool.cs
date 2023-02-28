#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelEditorTool : EditorWindow
{
    public static Ray ray;
    private static bool isNeededInitialize = true;
    public static RaycastHit hitResult;
    private static Tile_Edit selectedTile;
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
        Init();
        SceneView.duringSceneGui += OnSceneGUI;
    }
 
    [MenuItem("Window/Custom Tools/Level Editor/Disable")]
    public static void Disable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }
    public static void Init()
    {
        selectedTile = default;
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

        Raycast();
        Click();
        Handles.EndGUI();
    }

    private static void Raycast()
    {
        GFunc.Log("RayCast 쐈음");

        Event e = Event.current;
        ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 50, Color.red, 1.0f);
        if(Physics.Raycast(ray, out hitResult))
        {
            GFunc.Log("RayCast 뭔가 맞았는데 타일 아님");
            Tile_Edit eTile = hitResult.transform.gameObject.GetComponent<Tile_Edit>();
            if(eTile != null && eTile != default)
            {
                GFunc.Log("RayCast 타일 맞음");
                selectedTile = eTile;
                eTile.onMouseOver();
            }
        }
    }
    private static void Click()
    {
        Event e = Event.current;
        if(e.button == 0 && e.isMouse)
        {
            // Left Click
            GFunc.Log("Left Click");
            if(selectedTile == null || selectedTile == default)
                return;
            // 타일에 objectId 설정 작업
            GFunc.Log("Left Click and Setup Object");
            selectedTile.onSetupObject(ObjectListWindow.selectedObjId, ObjectListWindow.selectedTexture);
            selectedTile = default;

        }
        else if(e.button == 1)
        {
            // Right Click
            GFunc.Log("Right Click");
            ObjectListWindow.ResetSelected();
            GridController_Edit gridCtrl = GFunc.GetRootObj("GameObjs").FindChildObj("Grid").GetComponentMust<GridController_Edit>();
            gridCtrl.onPointingTile();
        }
    }
    
}

#endif