using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static partial class GFunc
{
    //! �ؽ�Ʈ�޽����� ������ ������Ʈ�� �ؽ�Ʈ�� �����ϴ� �Լ�
    public static void SetTmpText(this GameObject obj_, string text_)
    {
        TMP_Text tmpTxt = obj_.GetComponentInChildren<TMP_Text>();
        if (tmpTxt == null || tmpTxt == default(TMP_Text))
        {
            return;
        }       // if: ������ �ؽ�Ʈ�޽� ������Ʈ�� ���� ���

        // ������ �ؽ�Ʈ�޽� ������Ʈ�� �����ϴ� ���
        tmpTxt.text = text_;
        
    }       // SetTextMeshPro()
    public static void CreateWorldTextMesh(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color color = default(Color), TextAnchor textAnchor = TextAnchor.MiddleCenter, TextAlignment textAlignment = TextAlignment.Center, int sortingOrder = default)
    {
        if(color == null) color = Color.white;
        //return 
        CreateWorldTextMesh(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }
    public static void CreateWorldTextMesh(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        TMP_Text textMesh = default;
        GameObject gameObject = new GameObject("World_Text", typeof(TMP_Text));
        RectTransform transform = gameObject.GetRect();
        transform.parent = parent;
        transform.localPosition = localPosition;
        GFunc.Log("Check!");
        //TMP_Text textMesh = gameObject.GetComponentMust<TMP_Text>();
        
        //textMesh. = textAnchor;
        //textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        // textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        //return textMesh;
    }
}
