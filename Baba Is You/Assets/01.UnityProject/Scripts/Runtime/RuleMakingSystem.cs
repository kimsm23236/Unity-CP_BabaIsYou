using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleMakingSystem : MonoBehaviour
{
    private GridData curLevelGridData = default;
    private ObjectController objectController = default;
    private ObjectProperty[,] textObjArr = default;
    private List<ObjectProperty> listTextObj = default;
    private List<ObjectProperty> listVerb = default;
    private List<List<ObjectProperty>> currentRules = default;
    private List<List<ObjectProperty>> prevRules = default;
    private List<List<ObjectProperty>> tempRules = default;
    private List<ObjectProperty> makedRule = default;
    private bool isUpdateRule = default;

    private int gridWidth_ = default;
    private int gridHeight_ = default;

    public delegate void EventHandler();
    public EventHandler onRuleCheck;
    public EventHandler onUpdateRule;
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
        listTextObj = new List<ObjectProperty>();
        listVerb = new List<ObjectProperty>();
        onRuleCheck = new EventHandler(RuleCheck);
        onUpdateRule += () => isUpdateRule = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            UpdateList();
            curLevelGridData = GFunc.GetRootObj("GameObjs").FindChildObj("Grid").GetComponentMust<GridController>().gridData;
            gridWidth_ = curLevelGridData.width_;
            gridHeight_ = curLevelGridData.height_;
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            RuleCheck();
        }
        // 임시 룰 체킹 * 느려짐
        UpdateRuleCheck();
    }
    public void UpdateList()
    {
        listTextObj = UpdateListTObj();
        listVerb = UpdateListVerb();
        foreach(ObjectProperty od in listTextObj)
        {
            GFunc.Log($"TObj - {od.Name}");
        }
        foreach(ObjectProperty od in listVerb)
        {
            GFunc.Log($"Verb Obj - {od.Name}");
        }
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
    List<ObjectProperty> UpdateListTObj()
    {
        List<GameObject> AllObjs = objectController.Pool;
        List<ObjectProperty> newList = new List<ObjectProperty>();
        foreach(GameObject obj in AllObjs)
        {
            ObjectProperty opc = obj.GetComponentMust<ObjectProperty>();
            if(!opc.IsTextType())
            {
                continue;
            }
            newList.Add(opc);
        }
        return newList;
    }
    void InitTObjArray()
    {
        textObjArr = new ObjectProperty[gridHeight_, gridWidth_];
        foreach(ObjectProperty tObj in listTextObj)
        {
            int x = tObj.position.x;
            int y = tObj.position.y;
            textObjArr[y,x] = tObj;
            ColorSetter csc = tObj.gameObject.GetComponentMust<ColorSetter>();
            csc.onDeactivate();
        }
    }
    public void RuleCheck()
    {
        InitTObjArray();
        // 2차원 텍스트 오브젝트 배열 돌면서 오브젝트 타입을 만나면 DFS로 룰 검사
        for(int x = 0; x < textObjArr.GetLength(1); x++)
        {
            for(int y = 0; y < textObjArr.GetLength(0); y++)
            {
                // Debug.Log($"checking ... -> TextObject : {textObjArr[y,x].Name}");
                if(textObjArr[y,x] == null || textObjArr[y,x] == default)
                    continue;
                if(textObjArr[y,x].textType == TextType.Object)
                {
                    GFunc.Log($"Find TextObject : {textObjArr[y,x].Name}");
                    // 오른쪽 dfs 검사
                    DFS(x, y, 1, 0);
                    // 아래쪽 dfs 검사
                    DFS(x, y, 1, 1);
                }
            }
        }
        ApplyRule();
    }
    void DFS(int x, int y, int depth, int dir)
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
        makedRule.Add(textObjArr[y,x]);

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
            isRightSentence = textObjArr[y,x].textType == TextType.Object;
            break;
            case 2:
            isRightSentence = textObjArr[y,x].textType == TextType.Verb;
            break; 
            case 3:
            isRightSentence = textObjArr[y,x].textType == TextType.Object || textObjArr[y,x].textType == TextType.Attribute;
            break;
            default:
            isRightSentence = false;
            break;
        }

        return isRightSentence;
    }
    void AddRule()
    {
        tempRules.Add(makedRule);
    }
    void ApplyRule()
    {
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
        if(!isUpdateRule)
            return;
        isUpdateRule = false;
        RuleCheck();
    }
}

