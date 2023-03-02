using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    NONE = -1,
    InWorld, InLevel
}
public class GameManager : MonoBehaviour
{
    // singleton instance
    private static GameManager instance_ = null;
    private static bool isLoadLevel = false;
    private GameState state = default;
    private ObjectController objectController = default;
    private RuleMakingSystem ruleMakingSystem = default;

    private GameObject playerCursor = default;
    private GridPosition cursorPos = default;

    [SerializeField]
    private GameObject bg_World = default;
    [SerializeField]
    private GameObject bg_Level = default;
    public delegate void EventHandler_OneParam(int id);
    public EventHandler_OneParam onEnterLevel;

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
        if(instance_ == null)
        {
            instance_ = this;
        }
        else if(instance_ != this)
        {
            Destroy(gameObject);
        }
        // 배경 설정
        bg_Level.SetActive(false);
        bg_World.SetActive(false);
        DontDestroyOnLoad(gameObject);
        
        GFunc.Log("GameManager Awake");
    }
    // Start is called before the first frame update
    void Start()
    {
        // 월드맵 로드
        bg_World.SetActive(true);
        state = GameState.InWorld;
        //StageManager.Instance.SetStage(300);
        //StageManager.Instance.LoadStage();
        GameObject gameObjs = GFunc.GetRootObj("GameObjs");
        objectController = gameObjs.FindChildObj("ObjectController").GetComponentMust<ObjectController>();
        ruleMakingSystem = GFunc.GetRootObj("RuleMaker").GetComponentMust<RuleMakingSystem>();
        // 플레이어 시작 위치 설정
        cursorPos = new GridPosition();
        cursorPos.x = 9;
        cursorPos.y = 15;
        Req_LevelLoad(300);
        onEnterLevel = new EventHandler_OneParam(Req_LevelLoad);

    }

    // Update is called once per frame
    void Update()
    {
        LoadProcess();
    }
    void Req_LevelLoad(int id)
    {
        StageManager.Instance.SetStage(id);
        isLoadLevel = true;
        if(id == 300)
        {
            state = GameState.InWorld;
        }
        else
        {
            state = GameState.InLevel;
        }
    }
    void LoadProcess()
    {
        if(!isLoadLevel)
            return;
        isLoadLevel = false;

        if(state == GameState.InLevel)
        {
            bg_World.SetActive(false);
            bg_Level.SetActive(true);
            ObjectMovement omc = playerCursor.GetComponentMust<ObjectMovement>();
            cursorPos = omc.position;
        }

        StageManager.Instance.LoadStage();

        if(state == GameState.InWorld)
        {
            bg_World.SetActive(true);
            bg_Level.SetActive(false);
            playerCursor = objectController.CreateObject(31, cursorPos.x, cursorPos.y);
        }

        ruleMakingSystem.Init();
    }
}
