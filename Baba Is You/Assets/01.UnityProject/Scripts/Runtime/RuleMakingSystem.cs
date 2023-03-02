using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleMakingSystem : MonoBehaviour
{
    private GridData curLevelGridData = default;
    private ObjectController objectController = default;
    private ObjectMovement[,] textObjArr = default;
    private List<ObjectMovement> listTextObj = default;
    private List<ObjectProperty> listVerb = default;

    // 리팩토링 중 ....
    private List<Rule> rules = default;
    private List<Rule> updatedRules = default;
    private List<List<ObjectProperty>> currentRules = default;
    private List<List<ObjectProperty>> prevRules = default;
    private List<List<ObjectProperty>> tempRules = default;
    private List<ObjectProperty> makedRule = default;
    private bool isNeedUpdateRule = default;
    // ....

    private int gridWidth_ = default;
    private int gridHeight_ = default;

    public delegate void EventHandler();
    public delegate void EventHandler_Rule(Rule rule);
    public EventHandler onRuleCheck;
    public EventHandler onUpdateRule;
    public EventHandler_Rule onRemoveRule;
    // Start is called before the first frame update
    void Awake()
    {
        currentRules = new List<List<ObjectProperty>>();
        tempRules = new List<List<ObjectProperty>>();
        prevRules = new List<List<ObjectProperty>>();
        makedRule = new List<ObjectProperty>();
        GameObject gameObjs = GFunc.GetRootObj("GameObjs");
        objectController = gameObjs.FindChildObj("ObjectController").GetComponentMust<ObjectController>();

    }
    void Start()
    {
        listTextObj = new List<ObjectMovement>();
        listVerb = new List<ObjectProperty>();
        onRuleCheck = new EventHandler(UpdateRule);
        rules = new List<Rule>();
        isNeedUpdateRule = false;
        onUpdateRule += () => isNeedUpdateRule = true;
        onRemoveRule = new EventHandler_Rule(RemoveRule);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRule();
    }
    public void Init()
    {
        curLevelGridData = GFunc.GetRootObj("GameObjs").FindChildObj("Grid").GetComponentMust<GridController>().gridData;
        gridWidth_ = curLevelGridData.width_;
        gridHeight_ = curLevelGridData.height_;
        isNeedUpdateRule = true;
        RemoveAllRule();
        InitList();
    }
    public void InitList()
    {
        listTextObj = UpdateListTObj();
    }

    List<ObjectProperty> UpdateListVerb()
    {
        List<GameObject> AllObjs = objectController.Pool;
        List<ObjectProperty> newList = new List<ObjectProperty>();
        foreach(GameObject obj in AllObjs)
        {
            ObjectProperty opc = obj.GetComponentMust<ObjectProperty>();
            if(!opc.IsVerb())
            {
                continue;
            }
            newList.Add(opc);
        }
        return newList;
    }
    List<ObjectMovement> UpdateListTObj()
    {
        List<GameObject> AllObjs = objectController.Pool;
        List<ObjectMovement> newList = new List<ObjectMovement>();
        foreach(GameObject obj in AllObjs)
        {
            ObjectMovement omc = obj.GetComponentMust<ObjectMovement>();
            ObjectProperty opc = obj.GetComponentMust<ObjectProperty>();
            if(!opc.IsTextType())
            {
                continue;
            }
            omc.onMoved += () => isNeedUpdateRule = true;
            newList.Add(omc);
        }
        return newList;
    }
    void InitTObjArray()
    {
        textObjArr = new ObjectMovement[gridHeight_, gridWidth_];
        foreach(ObjectMovement tObj in listTextObj)
        {
            int x = tObj.position.x;
            int y = tObj.position.y;
            textObjArr[y,x] = tObj;
            GFunc.Log($"tobj : {textObjArr[y,x].gameObject.name}");
        }
    }
    public void UpdateRule()
    {
        if(!isNeedUpdateRule)
            return;
        isNeedUpdateRule = false; 

        InitTObjArray();
        // 2차원 텍스트 오브젝트 배열 돌면서 오브젝트 타입을 만나면 DFS로 룰 검사
        for(int x = 0; x < textObjArr.GetLength(1); x++)
        {
            for(int y = 0; y < textObjArr.GetLength(0); y++)
            {
                // Debug.Log($"checking ... -> TextObject : {textObjArr[y,x].Name}");
                if(textObjArr[y,x] == null || textObjArr[y,x] == default)
                    continue;
                if(textObjArr[y,x].objectProperty.textType == TextType.Object)
                {
                    GFunc.Log($"Find TextObject : {textObjArr[y,x].objectProperty.Name}");
                    // 오른쪽 dfs 검사
                    DFS(x, y, 1, 0);
                    // 아래쪽 dfs 검사
                    DFS(x, y, 1, 1);
                }
            }
        }
        ApplyRule();
    }
    public void DFS(int x, int y, int depth, int dir)
    {
        // 깊이 체크
        if(depth > 3) // 깊이 맥스값 변할수도 있음
            return;

        

        // default 체크
        if(textObjArr[y,x] == default)
            return;

        // 구문 검사
        // depth 별로 DFS 가능한 type을 리스트업?
        // 임시 작성
        if(!CheckSentence(x,y,depth))
            return;
        // Maked Rule add
        makedRule.Add(textObjArr[y,x].objectProperty);

        if(depth >= 3)
        {   
            GFunc.Log($"depth : {depth}, dir : {dir}");
            AddRule();
        }
        if(dir == 0)
        {
            // 오른쪽 검사
            DFS(x + 1, y, depth + 1, dir);
        }
        else if(dir == 1)
        {
            // 아래쪽 검사
            DFS(x, y + 1, depth + 1, dir);
        }
        makedRule = new List<ObjectProperty>();
    }
    bool CheckSentence(int x, int y, int depth)
    {
        bool isRightSentence = false;

        switch(depth)
        {
            case 1:
            isRightSentence = textObjArr[y,x].objectProperty.textType == TextType.Object;
            break;
            case 2:
            isRightSentence = textObjArr[y,x].objectProperty.textType == TextType.Verb;
            break; 
            case 3:
            isRightSentence = textObjArr[y,x].objectProperty.textType == TextType.Object || textObjArr[y,x].objectProperty.textType == TextType.Attribute;
            break;
            default:
            isRightSentence = false;
            break;
        }

        return isRightSentence;
    }
    void AddRule()
    {
        //
        Rule newRule = new Rule(makedRule[0], makedRule[1], makedRule[2]);
        bool isAddRule = true;
        foreach(Rule rule in rules)
        {
            if(rule == newRule)
            {
                isAddRule = false;
                break;
            }

        }
        if(!isAddRule)
            return;
        newRule.Init(this, objectController.Pool);
        rules.Add(newRule);
        //

        //updatedRules.Add(newRule);
        //tempRules.Add(makedRule);
    }
    void ApplyRule()
    {
        // 리팩토링 코드
        //if(updatedRules == rules)

        foreach(Rule rule in rules)
        {
            rule.Apply();
        }

        return;
        //
        objectController.AllObjAttributeReset();
        currentRules = tempRules;
        foreach(List<ObjectProperty> rule in currentRules)
        {
            string ruleString = string.Empty;
            for(int i = 0; i < rule.Count; i++)
            {
                ruleString += (rule[i].Name + " ");
                ColorSetter csc = rule[i].gameObject.GetComponentMust<ColorSetter>();
                csc.onActivate();
            }
            objectController.onExecuteRule(rule);
            GFunc.Log($"Rule : {ruleString}");
        }
        tempRules = new List<List<ObjectProperty>>();
    }
    void UpdateRuleCheck()
    {
        if(!isNeedUpdateRule)
            return;
        isNeedUpdateRule = false;
        UpdateRule();
    }
    void RemoveRule(Rule removed)
    {
        rules.Remove(removed);
    }
    public void RemoveAllRule()
    {
        rules = new List<Rule>();
    }
}

