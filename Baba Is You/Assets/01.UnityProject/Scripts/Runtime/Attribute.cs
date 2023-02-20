using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AtrType
{
    NONE = -1,
}
public class AttributeProperty
{
    public int id = default;
    public string name = default;
    public int priority = default;
}
public class Attribute
{
    public GameObject owner = default;
    public AttributeProperty property_ = default;
    public List<ObjectProperty> overlaps = default;
    public virtual void Attached(GameObject gObj_)
    {
        /* Do nothing */
    }
    public virtual void Execute()
    {
        /* Do nothing */
    }
    public virtual void OnOverlap()
    {
        GridPosition ownerPos = owner.GetComponentMust<ObjectProperty>().position;
        ObjectController occ = GFunc.GetRootObj("GameObjs").FindChildObj("ObjectController").GetComponentMust<ObjectController>();
        overlaps = new List<ObjectProperty>();
        foreach(GameObject obj in occ.Pool)
        {
            if(obj.GetComponentMust<ObjectProperty>().position == ownerPos)
            {
                overlaps.Add(obj.GetComponentMust<ObjectProperty>());
            }
        }
    }
}
