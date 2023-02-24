using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RuleType
{
    NONE = -1,
    SetAttribute, InitOBject
}
public class Rule
{
    private ObjectProperty rule_Subject_ = default;
    private ObjectProperty rule_Verb_ = default;
    private ObjectProperty rule_Object_ = default;
    private RuleType ruleType = RuleType.NONE;
    private string ruleName = string.Empty;
    private List<ObjectProperty> sbjOBjs = default;
    private RuleMakingSystem ruleMakingSystem = default;
    private bool isApplied = default;
    public Rule(ObjectProperty subject_, ObjectProperty verb_, ObjectProperty object_)
    {
        this.rule_Subject_ = subject_;
        this.rule_Verb_ = verb_;
        this.rule_Object_ = object_;
        isApplied = false;
        ruleName += subject_.Name + " " + verb_.Name + " " + object_.Name;
    }

    public void Init(RuleMakingSystem rms, List<GameObject> allObjs)
    {
        // 로그 출력
        GFunc.Log($"Rule Initilize : {ruleName}");
        // 룰 메이킹 시스템 캐싱
        ruleMakingSystem = rms;
        // 타입 설정
        if(rule_Object_.textType == TextType.Attribute)
            ruleType = RuleType.SetAttribute;
        else if(rule_Object_.textType == TextType.Object)
            ruleType = RuleType.InitOBject;

        sbjOBjs = new List<ObjectProperty>();
        // 주어에 해당하는 오브젝트를 캐싱
        foreach(GameObject obj in allObjs)
        {
            ObjectProperty opc = obj.GetComponentMust<ObjectProperty>();
            if(rule_Subject_.TagId == opc.id)
            {
                sbjOBjs.Add(opc);
            }
        }
        // 룰을 구성하는 텍스트 오브젝트의 무브 델리게이트에 자가 검사 함수 넣기
        rule_Subject_.gameObject.GetComponentMust<ObjectMovement>().onMoved += SelfVerify;
        rule_Verb_.gameObject.GetComponentMust<ObjectMovement>().onMoved += SelfVerify;
        rule_Object_.gameObject.GetComponentMust<ObjectMovement>().onMoved += SelfVerify;

    }
    public void Apply()
    {
        if(isApplied)
            return;

        isApplied = true;

        switch(ruleType)
        {
            case RuleType.SetAttribute:
            SetAttribute();
            break;
            case RuleType.InitOBject:
            InitObject();
            break;
            default:
            break;
        }

        ColorSetter csc = rule_Subject_.gameObject.GetComponentMust<ColorSetter>();
        csc.onActivate();
        csc = rule_Verb_.gameObject.GetComponentMust<ColorSetter>();
        csc.onActivate();
        csc = rule_Object_.gameObject.GetComponentMust<ColorSetter>();
        csc.onActivate();
    }
    void SetAttribute()
    {
        int atrId = rule_Object_.TagId;
        foreach(ObjectProperty opc in sbjOBjs)
        {
            opc.AddAttribute(atrId);
        }
    }
    void InitObject()
    {
        // 나중에
    }
    void SelfVerify()
    {
        if(!CheckLine())
        {
            // rms에 룰 제거 요청
            ruleMakingSystem.onRemoveRule(this);
            // 룰 해제에 따른 처리사항 * 속성 제거
            Release();
        }
    }
    void Release()
    {
        if(!isApplied)
            return;

        isApplied = false;

        int atrId = rule_Object_.TagId;
        foreach(ObjectProperty opc in sbjOBjs)
        {
            opc.AddRemoveAttribute(atrId);
        }
        // 불끄기
        ColorSetter csc = rule_Subject_.gameObject.GetComponentMust<ColorSetter>();
        csc.onDeactivate();
        csc = rule_Verb_.gameObject.GetComponentMust<ColorSetter>();
        csc.onDeactivate();
        csc = rule_Object_.gameObject.GetComponentMust<ColorSetter>();
        csc.onDeactivate();
        // 등록해두었던 함수 빼내기
        rule_Subject_.gameObject.GetComponentMust<ObjectMovement>().onMoved -= SelfVerify;
        rule_Verb_.gameObject.GetComponentMust<ObjectMovement>().onMoved -= SelfVerify;
        rule_Object_.gameObject.GetComponentMust<ObjectMovement>().onMoved -= SelfVerify;
    }
    bool CheckLine()
    {
        bool isEqualX = false;
        bool isEqualY = false;

        GridPosition sbjPos = rule_Subject_.gameObject.GetComponentMust<ObjectMovement>().position;
        GridPosition vrbPos = rule_Verb_.gameObject.GetComponentMust<ObjectMovement>().position;
        GridPosition objPos = rule_Object_.gameObject.GetComponentMust<ObjectMovement>().position;
        isEqualX = sbjPos.x == vrbPos.x && sbjPos.x == objPos.x;
        isEqualY = sbjPos.y == vrbPos.y && sbjPos.y == objPos.y;

        return isEqualX || isEqualY;
    }

    public static bool operator ==(Rule a, Rule b)
    {
        bool isEqualSbj = a.rule_Subject_.id == b.rule_Subject_.id;
        bool isEqualVerb = a.rule_Verb_.id == b.rule_Verb_.id;
        bool isEqualObj = a.rule_Object_.id == b.rule_Object_.id;
        return isEqualSbj && isEqualVerb && isEqualObj;
    }
    public static bool operator !=(Rule a, Rule b)
    {
        bool isEqualSbj = a.rule_Subject_.id != b.rule_Subject_.id;
        bool isEqualVerb = a.rule_Verb_.id != b.rule_Verb_.id;
        bool isEqualObj = a.rule_Object_.id != b.rule_Object_.id;
        return isEqualSbj || isEqualVerb || isEqualObj;
    }
}
