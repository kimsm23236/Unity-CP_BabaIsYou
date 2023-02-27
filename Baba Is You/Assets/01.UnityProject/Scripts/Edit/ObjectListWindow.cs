using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectListWindow : EditorWindow
{
    [SerializeField]
    public bool isNeededInitialize = true;

    private bool isPressed = true;
    private bool[] tempPressed = new bool[5] {true, false, false, false, false};
    public static void Init()
    {
        // 현재 활성화된 윈도우 가져오며, 없으면 새로 생성
        ObjectListWindow window = (ObjectListWindow)GetWindow(typeof(ObjectListWindow));
        window.Show();

        // 윈도우 타이틀 지정
        window.titleContent.text = "Object List";

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
        GUILayout.Label("Object List", EditorStyles.boldLabel);
        if(GUILayout.Button((Texture)Resources.Load("UseSprites/InStage/Object/baba_0_1"), GUILayout.Width(50),GUILayout.Height(50)))
        {
            GFunc.Log("Pressed Image Button!");
        }
        //if(GUILayout.Toggle(false, (Texture)Resources.Load("UseSprites/InStage/Object/baba_0_1"), "Button", GUILayout.Width(50),GUILayout.Height(50)))
        //{
        //    GFunc.Log("Pressed Image Button!");
        //}
        isPressed = GUILayout.Toggle(isPressed, (Texture)Resources.Load("UseSprites/InStage/Object/baba_0_1"), "Button", GUILayout.Width(50), GUILayout.Height(50));
        GUILayout.Space(10f);
        GUILayout.BeginHorizontal();
        Texture2D texture = (Texture2D)Resources.Load("UseSprites/InStage/Object/baba_0_1");
        
        for(int i = 0; i < 5; i++)
        {
            tempPressed[i] = GUILayout.Toggle(tempPressed[i], texture, "Button", GUILayout.Width(50), GUILayout.Height(50));
            if(tempPressed[i])
            {
                for(int j = 0 ; j < 5; j++)
                {
                    if(i == j)
                        continue;
                    tempPressed[j] = false;
                }
            }
        }
        GUILayout.EndHorizontal();

    }
}
