using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SetStagePropertyWindow : EditorWindow
{
    [SerializeField]
    private int levelId;
    [SerializeField]
    private string levelName;
    [SerializeField]
    public bool isNeededInitialize = true;
    [SerializeField]
    private StageProperty stageProperty;

    public static void Init()
    {
        // 현재 활성화된 윈도우 가져오며, 없으면 새로 생성
        SetStagePropertyWindow window = (SetStagePropertyWindow)GetWindow(typeof(SetStagePropertyWindow));
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
            isNeededInitialize = false;
        }
        
        // 굵은 글씨 
        Color originColor = EditorStyles.boldLabel.normal.textColor;
        EditorStyles.boldLabel.normal.textColor = Color.yellow;
        // Header =====================================================================
        GUILayout.Space(10f);
        GUILayout.Label("Stage Property", EditorStyles.boldLabel);

        levelId = EditorGUILayout.IntField("id", levelId);
        levelName = EditorGUILayout.TextField("name", levelName);

        // ============================================================================
        // GUILayout.Space(10f);
        // GUILayout.Label("Horizontal", EditorStyles.boldLabel);
        EditorStyles.boldLabel.normal.textColor = originColor;

        if(GUILayout.Button("Save Property"))
        {
            StageProperty newStageProperty = new StageProperty();
            newStageProperty.id = levelId;
            newStageProperty.name = levelName;
            stageProperty = newStageProperty;
        }
    }
}
