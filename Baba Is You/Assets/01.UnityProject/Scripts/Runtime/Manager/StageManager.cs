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
        if(instance_ == null)
        {
            instance_ = this;
        }
        else if(instance_ != this)
        {
            Destroy(gameObject);
        }
        GameObject gameObjs = GFunc.GetRootObj("GameObjs");
        canvasScaler = gameObjs.GetComponentMust<CanvasScaler>();
        gridController = gameObjs.FindChildObj("Grid").GetComponentMust<GridController>();
        objectController = gameObjs.FindChildObj("ObjectController").GetComponentMust<ObjectController>();
        DontDestroyOnLoad(gameObject);
        GFunc.Log("Stage Manager Awake");
    }
    void Start()
    {
        GameObject gameObjs = GFunc.GetRootObj("GameObjs");
        canvasScaler = gameObjs.GetComponentMust<CanvasScaler>();
        gridController = gameObjs.FindChildObj("Grid").GetComponentMust<GridController>();
        objectController = gameObjs.FindChildObj("ObjectController").GetComponentMust<ObjectController>();
        GFunc.Log("Stage Manager Start");
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
        {
            GFunc.Log("gridController is null");
        }  
        if(currentStage == null)
            GFunc.Log("currentStage is null");
        
        gridController.SetupGridData(currentStage);
        canvasScaler.referenceResolution = new Vector2(currentStage.col * 24, currentStage.row * 24);
        objectController.StageLoad(currentStage);
    }
}
