using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SetGridWindow : EditorWindow
{
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    [SerializeField]
    public bool isNeededInitialize = true;
    [SerializeField]
    private GridController gridController = default;
    public static void Init()
    {
        // 현재 활성화된 윈도우 가져오며, 없으면 새로 생성
        SetGridWindow window = (SetGridWindow)GetWindow(typeof(SetGridWindow));
        window.Show();

        // 윈도우 타이틀 지정
        window.titleContent.text = "Set Grid Data";

        // 최소, 최대 크기 지정
        window.minSize = new Vector2(340f, 150f);
        window.maxSize = new Vector2(340f, 200f);
    }

    void OnGUI()
    {
        if(isNeededInitialize)
        {
            gridController = GFunc.GetRootObj("GameObjs").FindChildObj("Grid").GetComponentMust<GridController>();
            gridController.Init_Editor();
            isNeededInitialize = false;
            GFunc.Log("Set Grid Window Init");
        }
        
        // 굵은 글씨 
        Color originColor = EditorStyles.boldLabel.normal.textColor;
        EditorStyles.boldLabel.normal.textColor = Color.yellow;
        // Header =====================================================================
        GUILayout.Space(10f);
        GUILayout.Label("Level Size", EditorStyles.boldLabel);

        width = EditorGUILayout.IntField("Width", width);
        height = EditorGUILayout.IntField("Height", height);

        // ============================================================================
        // GUILayout.Space(10f);
        // GUILayout.Label("Horizontal", EditorStyles.boldLabel);
        EditorStyles.boldLabel.normal.textColor = originColor;

        if(GUILayout.Button("Set Grid"))
        {
            StageProperty newStageProperty = new StageProperty();
            newStageProperty.col = width;
            newStageProperty.row = height;
            gridController.SetupGrid_Editor(width, height);
        }
    }
}
