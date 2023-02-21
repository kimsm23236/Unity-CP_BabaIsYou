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
    private float moveSpeed = 1000f;
    public GameObject owner = default;
    private ObjectProperty ownerOpc = default;
    public AttributeProperty property_ = default;
    public List<ObjectProperty> overlaps = default;
    protected List<GameObject> objectPool = default; 
    protected GridPosition position_ = default;
    public Transform movePoint = default;
    public virtual void Attached(GameObject gObj_)
    {
        owner = gObj_;
        ownerOpc = gObj_.GetComponentMust<ObjectProperty>();
        position_ = ownerOpc.position;
        movePoint = ownerOpc.movePoint;
        GameObject gameObj = GFunc.GetRootObj("GameObjs");
        objectPool = gameObj.FindChildObj("ObjectController").GetComponentMust<ObjectController>().Pool;
    }
    public virtual void Execute()
    {
        // Move();
        ownerOpc.position = position_;
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
    public void Move()
    {
        owner.transform.position = Vector3.MoveTowards(owner.transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        
    }
}
