using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtrHot : Attribute
{
    public AtrHot()
    {
        AttributeData atrData = DataManager.Instance.dicAtrData[6];
        property_ = new AttributeProperty();
        property_.id = atrData.id;
        property_.name = atrData.name;
        property_.priority = 0;
    }
    public override void Attached(GameObject gObj_)
    {
        base.Attached(gObj_);
        GameObject gameObj = GFunc.GetRootObj("GameObjs");
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
            // hot melt 겹침 작업
            if(opc.FindAttribute(7))
            {
                ICommand Dead = new Live(opc.gameObject, false, objectController.Turn - 1);
                objectController.PushLive(Dead);
            }
        }
    }
}