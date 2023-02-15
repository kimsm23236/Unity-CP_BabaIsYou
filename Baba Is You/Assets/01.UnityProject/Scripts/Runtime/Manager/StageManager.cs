using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private StageData stageData = default;
    private StageProperty currentStage = default;
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
    public void LoadStage(int id)
    {
        
    }
}
