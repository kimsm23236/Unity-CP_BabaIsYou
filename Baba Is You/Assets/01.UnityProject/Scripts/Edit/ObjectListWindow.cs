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
    public static int objCnt = default;
    public static List<bool> pressList = new List<bool>();
    public static List<Texture2D> textures = new List<Texture2D>();
    public static Texture2D selectedTexture = default;
    public static int selectedObjId = default;

    Rect rScrollRect;  // 화면상의 스크롤 뷰의 위치
    Rect rScrollArea; // 총 스크롤 되는 공간
    Vector2 vScrollPos; // 스크롤 바의 위치
    public static void Init()
    {
        // 현재 활성화된 윈도우 가져오며, 없으면 새로 생성
        ObjectListWindow window = (ObjectListWindow)GetWindow(typeof(ObjectListWindow));
        window.Show();
        objCnt = DataManager.Instance.dicObjData.Count;
        ResetSelected();
        for(int i = 0 ; i < objCnt; i++)
        {
            Texture2D texture = (Texture2D)Resources.Load(DataManager.Instance.dicObjData[i].sprite_path);
            textures.Add(texture);
            pressList.Add(false);
        }
        

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

        vScrollPos = GUILayout.BeginScrollView(vScrollPos);

        GUILayout.Space(10f);
        GUILayout.Label("Object List", EditorStyles.boldLabel);
    
        GUILayout.Space(10f);
        GUILayout.BeginHorizontal();
        for(int i = 0; i < objCnt; i++)
        {
            pressList[i] = GUILayout.Toggle(pressList[i], textures[i], "Button", GUILayout.Width(50), GUILayout.Height(50));
            if(pressList[i])
            {
                selectedTexture = textures[i];
                selectedObjId = i;
                for(int j = 0 ; j < objCnt; j++)
                {
                    if(i == j)
                        continue;
                    pressList[j] = false;
                }
            }
            if((i+1) % 5 == 0)
            {
                GUILayout.EndHorizontal();
                GUILayout.BeginVertical();
                GUILayout.Space(10f);
                GUILayout.EndVertical();
                GUILayout.BeginHorizontal();
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();

    }
    public static void ResetSelected()
    {
        selectedTexture = default;
        selectedObjId = -1;
    }
    public static bool IsObjectSelected()
    {
        bool isSeleted = true;

        isSeleted = selectedObjId >= 0;
        isSeleted = selectedTexture != null || selectedTexture != default;
        return isSeleted;
    }
}
