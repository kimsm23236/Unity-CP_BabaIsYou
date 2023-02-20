using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtrWin : Attribute
{
    List<GameObject> objectPool = default; 
    public AtrWin()
    {
        AttributeData atrData = DataManager.Instance.dicAtrData[1];
        property_ = new AttributeProperty();
        property_.id = atrData.id;
        property_.name = atrData.name;
        property_.priority = 0;
    }
    public override void Attached(GameObject gObj_)
    {
        owner = gObj_;
        GameObject gameObj = GFunc.GetRootObj("GameObjs");
        objectPool = gameObj.FindChildObj("ObjectController").GetComponentMust<ObjectController>().Pool;
    }
    public override void Execute()
    {
        OnOverlap();
    }
    public override void OnOverlap()
    {
        base.OnOverlap();
        foreach(ObjectProperty opc in overlaps)
        {
            // win you 겹침 작업
            if(opc.FindAttribute(0))
            {
                ClearLevel();
            }
        }
    }
    private void ClearLevel()
    {
        // 임시 게임 종료
        GFunc.QuitThisGame();
    }
}
