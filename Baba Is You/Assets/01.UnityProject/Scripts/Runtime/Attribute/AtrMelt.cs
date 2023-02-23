using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtrMelt : Attribute
{
    public AtrMelt()
    {
        AttributeData atrData = DataManager.Instance.dicAtrData[7];
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
}
