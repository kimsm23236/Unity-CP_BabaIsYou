using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AtrType
{
    NONE = -1,
}
public class AttributeProperty
{
    string name = default;
    int priority = default;
}
public abstract class Attribute : MonoBehaviour
{
    public AttributeProperty property_ = default;
    

}
