using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    private static StageManager instance_ = null;
    public static StageManager Instance
    {
        get
        {
            if(instance_ == null)
            {
                instance_ = new StageManager();
            }
            return instance_;
        }
    }
    private StageManager()
    {

    }
    private StageData stageData = default;
    private StageProperty currentStage = default;
    private CanvasScaler canvasScaler = default;
    private GridController gridController = default;
    private ObjectController objectController = default;
    void Awake()
    {
        GameObject gameObjs = GFunc.GetRootObj("GameObjs");
        canvasScaler = gameObjs.GetComponentMust<CanvasScaler>();
        gridController = gameObjs.FindChildObj("Grid").GetComponentMust<GridController>();
        objectController = gameObjs.FindChildObj("ObjectController").GetComponentMust<ObjectController>();
        GFunc.Log("Stage Manager Awake");
    }
    void Start()
    {
    }
    void Update()
    {
    }
    public void SetStage(int id)
    {
        stageData = DataManager.Instance.dicStgData[id];
        currentStage = stageData.Convert();
    }
    public void LoadStage()
    {
        if(gridController == null)
            GFunc.Log("gridController is null");
        if(currentStage == null)
            GFunc.Log("currentStage is null");
        GameObject gameObjs = GFunc.GetRootObj("GameObjs");
        canvasScaler = gameObjs.GetComponentMust<CanvasScaler>();
        gridController = gameObjs.FindChildObj("Grid").GetComponentMust<GridController>();
        objectController = gameObjs.FindChildObj("ObjectController").GetComponentMust<ObjectController>();
        gridController.SetupGridData(currentStage);
        canvasScaler.referenceResolution = new Vector2(currentStage.col * 24, currentStage.row * 24);
        objectController.StageLoad(currentStage);
    }
}
