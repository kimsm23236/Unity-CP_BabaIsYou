using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSetter : MonoBehaviour
{
    private SpriteRenderer sr = default;
    private Color currentColor_ = default;
    private Color baseColor_ = default;
    private Color activateColor_ = default;
    private Color deactivateColor_ = default;
    public delegate void EventHandler();
    public delegate void EventHandler_TwoParam(int value, ObjectType type);
    public EventHandler onActivate;
    public EventHandler onDeactivate;
    public EventHandler_TwoParam onInitObject;
    // Start is called before the first frame update
    void Awake()
    {
        sr = gameObject.GetComponentMust<SpriteRenderer>();
        onInitObject = new EventHandler_TwoParam(ChangedObject);
        onActivate += SetActivateColor;
        onDeactivate += SetDeactivateColor;
    }
    void Start()
    {
        
    }
    void ChangeColor()
    {
        GFunc.Log($"set Color : {currentColor_.r}, {currentColor_.g}, {currentColor_.b}, {currentColor_.a}");
        
        sr.color = currentColor_;
    }
    void SetActivateColor()
    {
        currentColor_ = activateColor_;
        ChangeColor();
    }
    void SetDeactivateColor()
    {
        currentColor_ = deactivateColor_;
        ChangeColor();
    }
    void SetBaseColor()
    {
        currentColor_ = baseColor_;
        ChangeColor();
    }
    void ChangedObject(int id, ObjectType type)
    {
        List<string> strArr = new List<string>();
        strArr.Add(DataManager.Instance.dicObjData[id].basecolor);
        strArr.Add(DataManager.Instance.dicObjData[id].activatecolor);
        strArr.Add(DataManager.Instance.dicObjData[id].deactivatecolor);
        List<Color> colorArr = new List<Color>();
        foreach(string str in strArr)
        {
            float r = DataManager.Instance.dicColorData[str].r / 255f;
            float g = DataManager.Instance.dicColorData[str].g / 255f;
            float b = DataManager.Instance.dicColorData[str].b / 255f;
            Color color = new Color(r, g, b);
            colorArr.Add(color);
        }
        baseColor_ = colorArr[0];
        activateColor_ = colorArr[1];
        deactivateColor_ = colorArr[2];

        if(type == ObjectType.Object)
            SetBaseColor();
        else
            SetDeactivateColor();
    }
}
