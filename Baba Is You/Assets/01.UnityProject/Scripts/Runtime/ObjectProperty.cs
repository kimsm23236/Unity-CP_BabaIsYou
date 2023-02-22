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
    private GridPosition position_ = default;
    public GridPosition position
    {
        get
        {
            return position_;
        }
        set
        {
            position_ = value;
        }
    }
    private Direction direction_ = Direction.Right;
    public Direction direction
    {
        get
        {
            return direction_;
        }
        set
        {
            direction_ = value;
        }
    }
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

    public Transform movePoint = default;

    private ColorSetter colorSetter = default;

    #region Attribute Member
    private List<Attribute> atrArr = default;
    public List<Attribute> attributes
    {
        get
        {
            return atrArr;
        }
    }

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
        GFunc.Log("colorSetter Setup");
        anim.enabled = false;
        anim.enabled = true;
        atrArr = new List<Attribute>();
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
        
        colorSetter.onInitObject(id, oType_);
        taggedId = data_.tag;
    }
    void Update()
    {
        InitTest();
        foreach(Attribute atr in atrArr)
        {
            atr.Execute();
        }
        Move();
    }
    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, 2000f * Time.deltaTime);
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
            default:
            break;
        }
        atr.Attached(gameObject);
        atrArr.Add(atr);
    }

    public void ResetAtrs()
    {
        atrArr = new List<Attribute>();
    }
}
