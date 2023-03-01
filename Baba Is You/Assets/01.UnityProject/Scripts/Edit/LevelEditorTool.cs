#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class LevelEditorTool : EditorWindow
{
    public static Ray ray;
    private static bool isNeededInitialize = true;
    public static RaycastHit hitResult;
    private static Tile_Edit selectedTile;
    private static GameObject actualObject_;
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

        GUILayout.Space(50f);
        if (GUILayout.Button("Save Level", GUILayout.Width(100), GUILayout.Height(50)))
        {
            JsonSave();
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
        IMouse isEditorTile;
        if(Physics.Raycast(ray, out hitResult))
        {
            GFunc.Log("RayCast 뭔가 맞았는데 타일 아님");
            GFunc.Log($"Hit Object : {hitResult.collider.gameObject.name}");
            if(actualObject_ == null)
            {
                GFunc.Log("Raycast 중 뭔가 충돌 -> actualGameObject is null");
                actualObject_ = hitResult.collider.gameObject;
                // actualObject의 마우스오버 함수
                isEditorTile = actualObject_.GetComponent<Tile_Edit>();
                if(isEditorTile != null || isEditorTile != default)
                {
                    isEditorTile.MouseEnter();
                }
            }
            else if(actualObject_ != hitResult.collider.gameObject)
            {
                GFunc.Log("Raycast 중 뭔가 충돌 -> actualGameObject is not null");
                // actualObject의 마우스나감 함수
                isEditorTile = actualObject_.GetComponent<Tile_Edit>();
                if(isEditorTile != null || isEditorTile != default)
                {
                    isEditorTile.MouseExit();
                }
                // actualObject 변경
                actualObject_ = hitResult.collider.gameObject;
                // actualObject의 마우스오버 함수
                isEditorTile = actualObject_.GetComponent<Tile_Edit>();
                if(isEditorTile != null || isEditorTile != default)
                {
                    isEditorTile.MouseEnter();
                }
            }
        }
        else
        {
            if(actualObject_ != null)
            {
                isEditorTile = actualObject_.GetComponent<Tile_Edit>();
                if(isEditorTile != null || isEditorTile != default)
                {
                    isEditorTile.MouseExit();
                }

                actualObject_ = default;
            }
        }
    }
    private static void Click()
    {
        if(actualObject_ == null || actualObject_ == default)
            return;
        IMouse actualTile = actualObject_.GetComponent<Tile_Edit>();

        if(!ObjectListWindow.IsObjectSelected())
            return;

        Event e = Event.current;
        if(e.button == 0 && e.type == EventType.MouseDown)
        {
            // Left Click
            GFunc.Log("Left Click");
            
            if(actualTile != null)
            {
                actualTile.MouseClick_L();
            }
        }
        else if(e.button == 1 && e.type == EventType.MouseDown)
        {
            // Right Click
            GFunc.Log("Right Click");
            if(actualTile != null)
            {
                actualTile.MouseClick_R();
            }
            ObjectListWindow.ResetSelected();
        }
    }
    public static void JsonSave()
    {
        StageData stageData = new StageData();
        stageData.id = SetStagePropertyWindow.stageProperty.id;
        stageData.name = SetStagePropertyWindow.stageProperty.name;
        stageData.col = SetGridWindow.GridWidth;
        stageData.row = SetGridWindow.GridHeight;
        int[,] objIdArr = new int[stageData.row, stageData.col];

        GridController_Edit grindController = GFunc.GetRootObj("GameObjs").FindChildObj("Grid").GetComponentMust<GridController_Edit>();

        for(int x = 0; x < stageData.col; x++)
        {
            for(int y = 0; y < stageData.row; y++)
            {
                objIdArr[y,x] = grindController.tiles[y,x].objectID;
            }
        }
        stageData.Convert_IntArrayToString(objIdArr);
        
        string json = JsonUtility.ToJson(stageData, true);
        string path = Path.Combine(Application.dataPath, "LevelData.json");
        File.WriteAllText(path, json);
        GFunc.Log($"Saved File : {path}");
    }
    
}

#endif