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
public abstract class Attribute
{
    public GameObject owner = default;
    public AttributeProperty property_ = default;
    public delegate void EventHandler(GameObject gObj_);
    public delegate bool bEventHandler(GameObject gObj_);
    public EventHandler onOverlapEvent;
    public bEventHandler onCollisionEvent;
    public abstract void Attached(GameObject gObj_);
    public abstract void Execute();
}
