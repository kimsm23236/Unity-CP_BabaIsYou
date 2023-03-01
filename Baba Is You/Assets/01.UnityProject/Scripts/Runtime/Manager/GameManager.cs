using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // singleton instance
    private static GameManager instance_ = null;

    private StageManager stageManager = default;

    // singleton access property
    public static GameManager Instance
    {
        get
        {
            if(instance_ == null)
            {
                instance_ = new GameManager();
            }
            return instance_;
        }
    }
    private GameManager()
    {

    }
    void Awake()
    {
        stageManager = StageManager.Instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        // 월드맵 로드
        stageManager.SetStage(300);
        stageManager.LoadStage();
        ObjectController objectController = GFunc.GetRootObj("GameObjs").FindChildObj("ObjectController").GetComponentMust<ObjectController>();
        objectController.CreateObject(31, 9, 15);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void CreateCursor()
    {

    }
}
