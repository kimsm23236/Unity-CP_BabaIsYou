using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    NONE = -1,
    Right,Down,Left,Up
}
public enum ObjectType
{
    NONE = -1,
    Object, Text
}
public class GridPosition
{
    public int x = default;
    public int y = default;
}
public class ObjectProperty : MonoBehaviour
{
    private Animator anim = default;
    private GridPosition position_ = default;
    private ObjectData data_ = default;
    private ObjectType type_ = default;

    void Awake()
    {
        anim = gameObject.GetComponentMust<Animator>();
        anim.enabled = false;
        anim.enabled = true;
    }
    void Start()
    {
        DataManager.Instance.ToString();
        InitObject(0);
    }
    public void InitObject(int id)
    {
        // 오브젝트 데이터 설정
        data_ = DataManager.Instance.dicObjData[id];
        GFunc.Log($"data : {data_.name}");
        // 데이터의 이름으로 타입과 애니메이션을 설정
        switch(data_.name[0])
        {
            case 'o':
            type_ = ObjectType.Object;
            anim.runtimeAnimatorController = DataManager.Instance.dicObjAnimData[data_.name];
            anim.SetInteger("Direction", (int)Direction.Right);
            break;
            case 't':
            type_ = ObjectType.Text;
            anim.runtimeAnimatorController = DataManager.Instance.toAnimData;
            anim.SetInteger("id", data_.id);
            break;
            default:
            type_ = ObjectType.NONE;
            break;
        }
    }
    void Update()
    {
        InitTest();
    }
    public void InitTest()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            InitObject(1);
        }
    }
}
