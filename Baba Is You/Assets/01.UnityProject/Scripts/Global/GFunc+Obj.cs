using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public static partial class GFunc
{
    //! Ư�� ������Ʈ�� �ڽ� ������Ʈ�� ��ġ�ؼ� ã���ִ� �Լ�
    public static GameObject FindChildObj(
        this GameObject targetObj_, string objName_)
    {
        GameObject searchResult = default;
        GameObject searchTarget = default;
        for (int i=0; i< targetObj_.transform.childCount; i++)
        {
            searchTarget = targetObj_.transform.GetChild(i).gameObject;
            if (searchTarget.name.Equals(objName_))
            {
                searchResult = searchTarget;
                return searchResult;
            }
            else
            {
                searchResult = FindChildObj(searchTarget, objName_);
                // ������
                if(searchResult == null || searchResult == default) { /* Pass */ }
                else { return searchResult; }
            }
        }       // loop
        return searchResult;
    }       // FindChildObj()


    //! ���� ��Ʈ ������Ʈ�� ��ġ�ؼ� ã���ִ� �Լ�
    public static GameObject GetRootObj(string objName_)
    {
        Scene activeScene_ = GetActiveScene();
        GameObject[] rootObjs_ = activeScene_.GetRootGameObjects();

        GameObject targetObj_ = default;
        foreach(GameObject rootObj in rootObjs_)
        {
            if(rootObj.name.Equals(objName_))
            {
                targetObj_ = rootObj;
                return targetObj_;
            }
            else { continue; }
        }       // loop

        return targetObj_;
    }       // GetRootObj()

    //! RectTransform ���� sizeDelta�� ã�Ƽ� �����ϴ� �Լ�
    public static Vector2 GetRectSizeDelta(this GameObject obj_)
    {
        return obj_.GetComponentMust<RectTransform>().sizeDelta;
    }       // GetRectSizeDelta()

    public static RectTransform GetRect(this GameObject obj_)
    {
        return obj_.GetComponentMust<RectTransform>();
    }
    public static void SetRectSizeDelta(this GameObject obj_, float newSize_)
    {
        RectTransform rectTransform = obj_.GetComponentMust<RectTransform>();
        rectTransform.sizeDelta = new Vector2(newSize_, newSize_);
    }

    //! ���� Ȱ��ȭ �Ǿ� �ִ� ���� ã���ִ� �Լ�
    public static Scene GetActiveScene()
    {
        Scene activeScene_ = SceneManager.GetActiveScene();
        return activeScene_;
    }       // GetActiveScene()

    //! ������Ʈ�� ���� �������� �����ϴ� �Լ�
    public static void SetLocalPos(this GameObject obj_, 
        float x, float y, float z)
    {
        obj_.transform.localPosition = new Vector3(x, y, z);
    }       // SetLocalPos()
    public static void SetLocalPos(this GameObject obj_, Vector3 newPos)
    {
        obj_.transform.localPosition = newPos;

    }

    //! ������Ʈ�� ���� �������� �����ϴ� �Լ�
    public static void AddLocalPos(this GameObject obj_, 
        float x, float y, float z)
    {
        obj_.transform.localPosition = 
            obj_.transform.localPosition + new Vector3(x, y, z);
    }       // AddLocalPos()

    public static void AddLocalPos(this GameObject obj_, 
        Vector3 moveVector)
    {
        obj_.transform.localPosition = 
            obj_.transform.localPosition + moveVector;
    }       // AddLocalPos()

    // ! 오브젝트 앵커 포지션을 연산하는 함수
    public static void AddAnchoredPos(this GameObject obj_, float x, float y)
    {
        obj_.GetRect().anchoredPosition += new Vector2(x, y);
    }

    // ! 오브젝트 앵커 포지션을 연산하는 함수
    public static void AddAnchoredPos(this GameObject obj_, Vector2 position2D)
    {
        obj_.GetRect().anchoredPosition += position2D;
    }
    public static void SetAnchoredPos(this GameObject obj_, Vector2 position2D)
    {
        obj_.GetRect().anchoredPosition = position2D;
    }

    //! Ʈ�������� ����ؼ� ������Ʈ�� �����̴� �Լ�
    public static void Translate(this Transform transform_, Vector2 moveVector)
    {
        transform_.Translate(moveVector.x, moveVector.y, 0f);
    }       // Translate()

    //! ������Ʈ �������� �Լ�
    public static T GetComponentMust<T>(this GameObject obj)
    {
        T component_ = obj.GetComponent<T>();

        GFunc.Assert(component_.IsValid<T>() != false, 
            string.Format("{0}���� {1}��(��) ã�� �� �����ϴ�.",
            obj.name, component_.GetType().Name));

        return component_;
    }       // GetComponentMust()
    public static T GetComponentMust<T>(this GameObject obj, string objName)
    {
        T component_ = obj.FindChildObj(objName).GetComponent<T>();

        GFunc.Assert(component_.IsValid<T>() != false, 
            string.Format("{0}���� {1}��(��) ã�� �� �����ϴ�.",
            obj.name, component_.GetType().Name));

        return component_;
    }       // GetComponentMust()

    // ! 새로운 오브젝트를 만들어서 컴포넌트를 리턴하는 함수
    public static T CreateObj<T>(string objName) where T : Component
    {
        GameObject newObj = new GameObject(objName);
        return newObj.AddComponent<T>();
    }
}
