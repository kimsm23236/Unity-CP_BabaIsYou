using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class ObjectController : MonoBehaviour
{
    private GameObject gameObjPrefab = default;
    private List<GameObject> objectPool = default;
    public List<GameObject> Pool
    {
        get
        {
            return objectPool;
        }
    }
    private GridController gridController = default;
    
    #region  Object Move
    private Stack<Move> moveStack = new Stack<Move>();

    public void PushMove(ICommand command)
    {
        moveStack.Push(command as Move);
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
        // 집가서 수정 * 푸쉬에서 플레이어 움직임이 마지막으로 들어가서 Undo 할때 플레이어만 따로 뺌 

        bool isLoopEnd = false;
        
        while(moveStack.Count >= 0 && !isLoopEnd)
        {
            Move lastMove = moveStack.Pop();
            lastMove.Undo();
            // 능동적 움직임이면 루프 탈출 * 플레이어의 움직임 등
            if(lastMove.isActiveMove)
                isLoopEnd = true;
        }
    }

    #endregion

    // EventHandler
    
    public delegate bool bEventHandler_Rule(List<ObjectProperty> rule);
    public bEventHandler_Rule onExecuteRule;
    //

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameObjs = GFunc.GetRootObj("GameObjs");
        gridController = gameObjs.FindChildObj("Grid").GetComponentMust<GridController>();
        gameObjPrefab = gameObject.FindChildObj("Object");
        gameObjPrefab.SetActive(false);
        objectPool = new List<GameObject>();
        onExecuteRule = new bEventHandler_Rule(ExecuteRule);
    }

    // Update is called once per frame
    void Update()
    {
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
                    opc.movePoint = newObj.FindChildObj("MovePoint").transform;
                    opc.movePoint.SetParent(gridController.transform);
                    GridPosition pos = new GridPosition();
                    pos.x = x;
                    pos.y = y;
                    opc.id = id;
                    opc.position = pos;
                    opc.movePoint.position = gridController.gridObjs[y, x].position;
                    objectPool.Add(newObj);
                }
            }
        }
    }
    public void SetupObjectData()
    {
        // 오브젝트 데이터 셋업
        foreach(GameObject obj in objectPool)
        {
            ObjectProperty op = obj.GetComponentMust<ObjectProperty>();
            int x = op.position.x;
            int y = op.position.y;
            op.InitObject();
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
            ObjectProperty opc = obj.GetComponentMust<ObjectProperty>();
            if(opc.position == pos)
            {
                objProps.Add(opc);
            }
        }
        return objProps;
    }

}
