using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtrStop : Attribute
{
    public AtrStop()
    {
        AttributeData atrData = DataManager.Instance.dicAtrData[2];
        property_ = new AttributeProperty();
        property_.id = atrData.id;
        property_.name = atrData.name;
        property_.priority = 0;
    }
}
