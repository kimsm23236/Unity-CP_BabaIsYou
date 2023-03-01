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
public enum TextType
{
    NONE = -1,
    Object, Verb, Attribute
}
public class GridPosition
{
    public int x = default;
    public int y = default;

    public static bool operator ==(GridPosition a, GridPosition b)
    {
        return a.x == b.x && a.y == b.y;
    }
    public static bool operator !=(GridPosition a, GridPosition b)
    {
        return a.x != b.x || a.y != b.y;
    }
}
public class ObjectProperty : MonoBehaviour
{
    public string Name
    {
        get
        {
            return data_.name;
        }
    }
    private Animator anim = default;
    
    private ObjectData data_ = default;
    private ObjectType oType_ = default;
    public ObjectType objectType
    {
        get
        {
            return oType_;
        }
    }
    private TextType tType_ = default;
    public TextType textType
    {
        get
        {
            return tType_;
        }
    }
    private int taggedId = -1;
    public int TagId
    {
        get
        {
            return taggedId;
        }
    }
    private int id_ = default;
    public int id
    {
        get
        {
            return id_;
        }
        set
        {
            id_ = value;
        }
    }

    private ColorSetter colorSetter = default;
    private ObjectTiling objectTiling = default;

    #region Attribute Member
    private List<Attribute> atrArr = default;
    public List<Attribute> attributes
    {
        get
        {
            return atrArr;
        }
    }
    private List<Attribute> removeAtrs = default;

    public bool FindAttribute(int id)
    {
        bool isSuccessFind = false;
        foreach(Attribute atr in atrArr)
        {
            if(id == atr.property_.id)
            {
                isSuccessFind = true;
                break;
            }
        }
        return isSuccessFind;
    }
    public bool FindAttribute(string name)
    {
        bool isSuccessFind = false;
        foreach(Attribute atr in atrArr)
        {
            if(name == atr.property_.name)
            {
                isSuccessFind = true;
                break;
            }
        }
        return isSuccessFind;
    }

    public Attribute GetAttribute(int id)
    {
        foreach(Attribute atr in atrArr)
        {
            if(atr.property_.id == id)
            {
                return atr;
            }
        }

        return default;
    }
    #endregion

    void Awake()
    {
        anim = gameObject.GetComponentMust<Animator>();
        colorSetter = gameObject.GetComponentMust<ColorSetter>();
        objectTiling = gameObject.GetComponentMust<ObjectTiling>();
        GFunc.Log("colorSetter Setup");
        anim.enabled = false;
        anim.enabled = true;
        atrArr = new List<Attribute>();
        removeAtrs = new List<Attribute>();
    }
    void Start()
    {
        DataManager.Instance.ToString();

    }
    public void InitObject()
    {
        // 오브젝트 데이터 설정
        data_ = DataManager.Instance.dicObjData[id];
        GFunc.Log($"data : {data_.name}");
        // 데이터의 이름으로 타입과 애니메이션을 설정
        switch(data_.otype)
        {
            case "o":
            oType_ = ObjectType.Object;
            anim.runtimeAnimatorController = DataManager.Instance.dicObjAnimData[data_.name];
            anim.SetInteger("Direction", (int)Direction.Right);
            break;
            case "t":
            oType_ = ObjectType.Text;
            anim.runtimeAnimatorController = DataManager.Instance.toAnimData;
            anim.SetInteger("id", data_.id);
            // Text 블록일 경우 기본으로 Push속성을 가지고 있음
            AddAttribute(3);
            break;
            default:
            break;
        }
        switch(data_.ttype)
        {
            case "n":
            tType_ = TextType.NONE;
            break;
            case "o":
            tType_ = TextType.Object;
            break;
            case "v":
            tType_ = TextType.Verb;
            break;
            case "a":
            tType_ = TextType.Attribute;
            break;
            default:
            tType_ = TextType.NONE;
            break;
        }
        if(id_ == 31)
        {
            AddAttribute(8);
        }
        
        colorSetter.onInitObject(id, oType_);
        objectTiling.onInitObject();
        
        taggedId = data_.tag;
    }
    void Update()
    {
        InitTest();
        foreach(Attribute atr in atrArr)
        {
            atr.Execute();
        }
        RemoveAttribute();
    }
    public void InitTest()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            InitObject();
        }
    }
    public bool IsVerb()
    {
        return tType_ == TextType.Verb;
    }
    public bool IsTextType()
    {
        return oType_ == ObjectType.Text;
    }
    public void AddAttribute(int atrId)
    {
        Attribute atr = default;
        // 속성 추가할 때 마다 추가작성
        switch(atrId)
        {
            case 0:
            atr = new AtrYou();
            break;
            case 1:
            atr = new AtrWin();
            break;
            case 2:
            atr = new AtrStop();
            break;
            case 3:
            atr = new AtrPush();
            break;
            case 4:
            atr = new AtrSink();
            break;
            case 5:
            atr = new AtrDefeat();
            break;
            case 6:
            atr = new AtrHot();
            break;
            case 7:
            atr = new AtrMelt();
            break;
            case 8:
            atr = new AtrCursor();
            break;
            case 9:
            // atr = new AtrSink();
            break;
            default:
            break;
        }
        atr.Attached(gameObject);
        atrArr.Add(atr);
    }
    public bool AddRemoveAttribute(int id)
    {
        bool isSuccessAddRemove = false;

        if(!FindAttribute(id))
        {
            return false;
        }

        Attribute removeAtr = default;
        foreach(Attribute atr in atrArr)
        {
            if(atr.Equals(id))
            {
                removeAtr = atr;
                break;
            }
        }

        removeAtrs.Add(removeAtr);
        isSuccessAddRemove = true;
        return isSuccessAddRemove;
    }
    public void RemoveAttribute()
    {
        if(removeAtrs.Count <= 0)
            return;

        atrArr.RemoveAll(removeAtrs.Contains);
        removeAtrs = new List<Attribute>();
    }

    public void ResetAtrs()
    {
        atrArr = new List<Attribute>();
    }
}
