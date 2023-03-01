using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class ObjectController : MonoBehaviour
{
    private GameObject gameObjPrefab = default;
    private List<GameObject> objectPool = default;
    private RuleMakingSystem ruleMakingSystem = default;
    public List<GameObject> Pool
    {
        get
        {
            return objectPool;
        }
    }
    private GridController gridController = default;
    
    #region  Object Move, Live
    private int turnCount = default;
    private bool isTurnIncrease = false;
    private bool isTurnDecrease = false;
    public int Turn
    {
        get
        {
            return turnCount;
        }
    }
    private Stack<Move> moveStack = new Stack<Move>();
    private Stack<Live> liveStack = new Stack<Live>();

    public void PushMove(ICommand command)
    {
        if(isTurnIncrease == false)
        {
            isTurnIncrease = true;
        }
        GFunc.Log($"move push stack, turnCount : {turnCount}");
        moveStack.Push(command as Move);
        command.Execute();
    }
    public void PushLive(ICommand command)
    {
        GFunc.Log($"live push stack, turnCount : {turnCount}");
        liveStack.Push(command as Live);
        command.Execute();
    }

    public void UndoMove()
    {
        /*
        if(commandList.Count <= 0)
            return;
        commandList[commandList.Count - 1].Undo();
        commandList.RemoveAt(commandList.Count - 1);
        */

        GFunc.Log("undo");

        while(liveStack.Count >= 1)
        {
            if(liveStack.First().turn_ != turnCount - 1)
                break;
            GFunc.Log($"live stack undo, turnCount : {turnCount}");
            Live lastLive = liveStack.Pop();
            lastLive.Undo();
        }

        while(moveStack.Count >= 1)
        {
            if(moveStack.First().turn != turnCount - 1)
                break;

            if(isTurnDecrease == false)
            {
                isTurnDecrease = true;
            }

            GFunc.Log($"move stack undo, turnCount : {turnCount}");
            Move lastMove = moveStack.Pop();
            lastMove.Undo();
        }
    }

    void IncreaseTurn()
    {
        if(!isTurnIncrease)
            return;
        isTurnIncrease = false;

        turnCount++;
        onChangedTurn();
        
    }

    void DecreaseTurn()
    {
        if(!isTurnDecrease)
            return;
        isTurnDecrease = false;
        turnCount = Mathf.Clamp(turnCount - 1, 0, int.MaxValue);
        onChangedTurn();
    }
    #endregion

    // EventHandler
    
    public delegate bool bEventHandler_Rule(List<ObjectProperty> rule);
    public delegate void EventHandler();
    public bEventHandler_Rule onExecuteRule;
    public EventHandler onChangedTurn;
    //

    // caching object
    private List<ObjectTiling> tilingObjects = default;
    //

    // Start is called before the first frame update
    void Awake()
    {
        GameObject gameObjs = GFunc.GetRootObj("GameObjs");
        gridController = gameObjs.FindChildObj("Grid").GetComponentMust<GridController>();
        ruleMakingSystem = GFunc.GetRootObj("TempRMS").GetComponentMust<RuleMakingSystem>();
        gameObjPrefab = gameObject.FindChildObj("Object");
        gameObjPrefab.SetActive(false);
        objectPool = new List<GameObject>();
        tilingObjects = new List<ObjectTiling>();
        onExecuteRule = new bEventHandler_Rule(ExecuteRule);
        onChangedTurn = () => GFunc.Log("Change Turn");
        onChangedTurn += () => ruleMakingSystem.onUpdateRule();
        onChangedTurn += AllTilingObjsTiling;
        //StartCoroutine(IncreaseTurn());
        //StartCoroutine(DecreaseTurn());
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        IncreaseTurn();
        DecreaseTurn();
        if(Input.GetKeyDown(KeyCode.Z))
        {
            UndoMove();
        }
    }
    

    // { stage load process
    public void StageLoad(StageProperty stageProperty)
    {
        CreateObject(stageProperty);
        SetupObjectData();
    }
    public void CreateObject(StageProperty stageProperty)
    {
        string[,] objsStrArr = stageProperty.objs;
        for(int y = 0; y < stageProperty.row; y++)
        {
            for(int x = 0; x < stageProperty.col; x++)
            {
                int id = default;
                bool isSuccess = int.TryParse(objsStrArr[y,x], out id);
                if(id >= 0)
                {
                    GameObject newObj = Instantiate(gameObjPrefab, transform);
                    newObj.transform.SetParent(gameObject.transform);
                    newObj.SetActive(true);
                    // movePoint = gameObject.FindChildObj("MovePoint").transform;
                    // movePoint.parent = GFunc.GetRootObj("GameObjs").FindChildObj("Grid").transform;
                    ObjectProperty opc = newObj.GetComponentMust<ObjectProperty>();
                    ObjectMovement omc = newObj.GetComponentMust<ObjectMovement>();
                    ObjectTiling otc = newObj.GetComponentMust<ObjectTiling>();
                    omc.movePoint = newObj.FindChildObj("MovePoint").transform;
                    omc.movePoint.SetParent(gridController.transform);
                    GridPosition pos = new GridPosition();
                    pos.x = x;
                    pos.y = y;
                    opc.id = id;
                    if(GData.OBJ_ID_TILING.Contains(id))
                    {
                        tilingObjects.Add(otc);
                    }
                    omc.position = pos;
                    omc.movePoint.position = gridController.gridObjs[y, x].position;
                    objectPool.Add(newObj);
                }
            }
        }
    }
    public void CreateObject(int id, int x, int y)
    {
        GameObject newObj = Instantiate(gameObjPrefab, transform);
        newObj.transform.SetParent(gameObject.transform);
        newObj.SetActive(true);

        ObjectProperty opc = newObj.GetComponentMust<ObjectProperty>();
        ObjectMovement omc = newObj.GetComponentMust<ObjectMovement>();
        ObjectTiling otc = newObj.GetComponentMust<ObjectTiling>();
        omc.movePoint = newObj.FindChildObj("MovePoint").transform;
        omc.movePoint.SetParent(gridController.transform);

        GridPosition pos = new GridPosition();
        pos.x = x;
        pos.y = y;
        opc.id = id;

        omc.position = pos;
        omc.movePoint.position = gridController.gridObjs[y, x].position;
        objectPool.Add(newObj);
        opc.InitObject();
        newObj.SetLocalPos(gridController.gridObjs[y,x].transform.localPosition);
    }
    public void SetupObjectData()
    {
        // 오브젝트 데이터 셋업
        foreach(GameObject obj in objectPool)
        {
            ObjectProperty opc = obj.GetComponentMust<ObjectProperty>();
            ObjectMovement omc = obj.GetComponentMust<ObjectMovement>();
            int x = omc.position.x;
            int y = omc.position.y;
            opc.InitObject();
            obj.SetLocalPos(gridController.gridObjs[y,x].transform.localPosition);
        }

    }
    // } stage load process

    public bool ExecuteRule(List<ObjectProperty> rule)
    {
        // 속성의 종류가 늘어날때마다 수정해야할것같음
        bool isSuccessExecute = true;


        // rule의 마지막이 속성인지 오브젝트인지 체크
        bool isAtr = rule.Last().textType == TextType.Attribute;
        if(isAtr)   // 속성인 경우
        {
            // rule의 첫번째 id값이 가르키는 오브젝트에 속성id값에따라 속성 추가
            foreach(GameObject obj in objectPool)
            {
                ObjectProperty opc = obj.GetComponentMust<ObjectProperty>();
                
                // TagId 룰에서 주어가 가리키는 오브젝트의 id 
                if(rule[0].TagId == opc.id)
                {
                    // TagId 목적어인 속성의 id를 가리킴
                    if(opc.FindAttribute(rule.Last().TagId))
                    {
                        continue;
                    }
                    opc.AddAttribute(rule.Last().TagId);
                }      
            }
        }
        else    // 오브젝트인 경우
        {

        }
        return isSuccessExecute;
    }
    public void AllObjAttributeReset()
    {
        foreach(GameObject obj in objectPool)
        {
            ObjectProperty opc = obj.GetComponentMust<ObjectProperty>();
            opc.ResetAtrs();
            if(opc.objectType == ObjectType.Text)
            {
                opc.AddAttribute(3);
            }
        }
    }
    public List<ObjectProperty> GetObjPropByPos(GridPosition pos)
    {
        List<ObjectProperty> objProps = new List<ObjectProperty>();
        foreach(GameObject obj in objectPool)
        {
            ObjectMovement omc = obj.GetComponentMust<ObjectMovement>();
            if(omc.position == pos)
            {
                ObjectProperty opc = omc.gameObject.GetComponentMust<ObjectProperty>();
                objProps.Add(opc);
            }
        }
        return objProps;
    }
    void AllTilingObjsTiling()
    {
        foreach(ObjectTiling otc in tilingObjects)
        {
            otc.onTiling();
        }
    }
}
