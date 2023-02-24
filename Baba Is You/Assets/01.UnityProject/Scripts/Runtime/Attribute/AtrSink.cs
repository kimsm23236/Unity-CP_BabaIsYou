using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtrSink : Attribute
{
    public AtrSink()
    {
        AttributeData atrData = DataManager.Instance.dicAtrData[4];
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
        
        bool isOnOverlap = false;
        foreach(ObjectProperty opc in overlaps)
        {
            if(owner == opc.gameObject)
                continue;

            isOnOverlap = true;
            ICommand Dead = new Live(opc.gameObject, false, objectController.Turn - 1);
            objectController.PushLive(Dead);
        }
        if(isOnOverlap)
        {
            ICommand Dead = new Live(owner, false, objectController.Turn - 1);
            objectController.PushLive(Dead);
        }
    }

}
