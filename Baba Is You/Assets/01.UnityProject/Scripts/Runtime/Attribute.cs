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
    protected ObjectProperty ownerOpc = default;
    protected ObjectMovement ownerOmc = default;
    [SerializeField]
    public AttributeProperty property_ = default;
    public List<ObjectProperty> overlaps = default;
    protected List<GameObject> objectPool = default; 
    protected GridPosition position_ = default;
    public Transform movePoint = default;
    public ObjectController objectController = default;
    public virtual void Attached(GameObject gObj_)
    {
        owner = gObj_;
        ownerOpc = gObj_.GetComponentMust<ObjectProperty>();
        ownerOmc = gObj_.GetComponentMust<ObjectMovement>();

        position_ = ownerOmc.position;
        movePoint = ownerOmc.movePoint;
        GameObject gameObj = GFunc.GetRootObj("GameObjs");
        objectPool = gameObj.FindChildObj("ObjectController").GetComponentMust<ObjectController>().Pool;
        objectController = gameObj.FindChildObj("ObjectController").GetComponentMust<ObjectController>();
        overlaps = new List<ObjectProperty>();
    }
    public virtual void Execute()
    {
        
    }
    public virtual void OnOverlap()
    {
        GridPosition ownerPos = ownerOmc.position;
        //ObjectController occ = GFunc.GetRootObj("GameObjs").FindChildObj("ObjectController").GetComponentMust<ObjectController>();
        
        foreach(GameObject obj in objectPool)
        {
            if(obj.GetComponentMust<ObjectMovement>().position == ownerPos)
            {
                overlaps.Add(obj.GetComponentMust<ObjectProperty>());
            }
        }
    }
    public bool Equals(int id)
    {
        return property_.id == id;
    }
}
