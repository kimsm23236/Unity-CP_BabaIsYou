using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtrWin : Attribute
{
    
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
        base.Attached(gObj_);
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
                GameManager.Instance.onEnterLevel(300, LoadType.ToWorld);
            }
        }
    }
    private void ClearLevel()
    {
        // 임시 게임 종료
        GFunc.QuitThisGame();
    }
}
