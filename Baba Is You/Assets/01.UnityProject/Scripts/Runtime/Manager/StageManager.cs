using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private StageData stageData = default;
    private StageProperty currentStage = default;
    private GridController gridController = default;
    private ObjectController objectController = default;
    void Awake()
    {
        GameObject gameObjs = GFunc.GetRootObj("GameObjs");
        gridController = gameObjs.FindChildObj("Grid").GetComponentMust<GridController>();
        objectController = gameObjs.FindChildObj("ObjectController").GetComponentMust<ObjectController>();
    }
    void Start()
    {
        SetStage(0);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            LoadStage();
        }
    }
    public void SetStage(int id)
    {
        stageData = DataManager.Instance.dicStgData[id];
        currentStage = stageData.Convert();
    }
    public void LoadStage()
    {
        gridController.SetupGridData(currentStage);
        objectController.StageLoad(currentStage);
    }
}
