using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleMakingSystem : MonoBehaviour
{
    private ObjectController objectController = default;
    private List<ObjectProperty> listTextObj = default;
    private List<ObjectProperty> listVerb = default;
    // Start is called before the first frame update
    void Awake()
    {
        GameObject gameObjs = GFunc.GetRootObj("GameObjs");
        objectController = gameObjs.FindChildObj("ObjectController").GetComponentMust<ObjectController>();
    }
    void Start()
    {
        listTextObj = new List<ObjectProperty>();
        listVerb = new List<ObjectProperty>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            UpdateList();
        }
    }
    public void UpdateList()
    {
        listTextObj = UpdateListTObj();
        listVerb = UpdateListVerb();
        foreach(ObjectProperty od in listTextObj)
        {
            GFunc.Log($"TObj - {od.Name}");
        }
        foreach(ObjectProperty od in listVerb)
        {
            GFunc.Log($"Verb Obj - {od.Name}");
        }
    }

    List<ObjectProperty> UpdateListVerb()
    {
        List<GameObject> AllObjs = objectController.Pool;
        List<ObjectProperty> newList = new List<ObjectProperty>();
        foreach(GameObject obj in AllObjs)
        {
            ObjectProperty opc = obj.GetComponentMust<ObjectProperty>();
            if(!opc.IsVerb())
            {
                continue;
            }
            newList.Add(opc);
        }
        return newList;
    }
    List<ObjectProperty> UpdateListTObj()
    {
        List<GameObject> AllObjs = objectController.Pool;
        List<ObjectProperty> newList = new List<ObjectProperty>();
        foreach(GameObject obj in AllObjs)
        {
            ObjectProperty opc = obj.GetComponentMust<ObjectProperty>();
            if(!opc.IsTextType())
            {
                continue;
            }
            newList.Add(opc);
        }
        return newList;
    }
}
