using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LoadType
{
    NONE = -1,
    ToWorld, ToLevel, RestartLevel
}
public class GameManager : MonoBehaviour
{
    // singleton instance
    private static GameManager instance_ = null;
    private static bool isLoadLevel = false;
    public static int currentLevelId = -1;
    private LoadType loadType = default;
    private ObjectController objectController = default;
    private RuleMakingSystem ruleMakingSystem = default;

    private GameObject playerCursor = default;
    private GridPosition cursorPos = default;

    [SerializeField]
    private GameObject bg_World = default;
    [SerializeField]
    private GameObject bg_Level = default;
    private bool isNeedInit = default;
    public delegate void EventHandler();
    public delegate void EventHandler_TwoParam(int id, LoadType type);
    public EventHandler onInitGameManager;
    public EventHandler_TwoParam onEnterLevel;

    // UI
    public PopupUI popupUI = default;
    //

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
        // DontDestroyOnLoad(gameObject);
        onInitGameManager = () => isNeedInit = true;
        isNeedInit = false;
        GFunc.Log("GameManager Awake");
    }
    // Start is called before the first frame update
    void Start()
    {
        // 월드맵 로드
        bg_World.SetActive(true);
        loadType = LoadType.ToWorld;
        //StageManager.Instance.SetStage(300);
        //StageManager.Instance.LoadStage();
        GameObject gameObjs = GFunc.GetRootObj("GameObjs");
        objectController = gameObjs.FindChildObj("ObjectController").GetComponentMust<ObjectController>();
        ruleMakingSystem = GFunc.GetRootObj("RuleMaker").GetComponentMust<RuleMakingSystem>();
        // 플레이어 시작 위치 설정
        cursorPos = new GridPosition();
        cursorPos.x = 9;
        cursorPos.y = 15;
        //Req_LevelLoad(300, LoadType.ToWorld);
        onEnterLevel = new EventHandler_TwoParam(Req_LevelLoad);
        onEnterLevel(300, LoadType.ToWorld);
        GameObject uiObjs = GFunc.GetRootObj("UiObjs");
        popupUI = uiObjs.FindChildObj("PausePopup").GetComponentMust<PopupUI>();
    }
    public void Init()
    {
        isNeedInit = false;
        // 월드맵 로드
        bg_World.SetActive(true);
        loadType = LoadType.ToWorld;
        //StageManager.Instance.SetStage(300);
        //StageManager.Instance.LoadStage();
        GameObject gameObjs = GFunc.GetRootObj("GameObjs");
        objectController = gameObjs.FindChildObj("ObjectController").GetComponentMust<ObjectController>();
        ruleMakingSystem = GFunc.GetRootObj("RuleMaker").GetComponentMust<RuleMakingSystem>();
        // 플레이어 시작 위치 설정
        cursorPos = new GridPosition();
        cursorPos.x = 9;
        cursorPos.y = 15;
        //Req_LevelLoad(300, LoadType.ToWorld);
        onEnterLevel = new EventHandler_TwoParam(Req_LevelLoad);
        onEnterLevel(300, LoadType.ToWorld);
    }

    // Update is called once per frame
    void Update()
    {
        LoadProcess();
    }
    void Req_LevelLoad(int id, LoadType type)
    {
        StageManager.Instance.SetStage(id);
        currentLevelId = id;
        isLoadLevel = true;
        loadType = type;
    }
    void LoadProcess()
    {
        if(!isLoadLevel)
            return;
        isLoadLevel = false;

        if(loadType == LoadType.ToLevel)
        {
            bg_World.SetActive(false);
            bg_Level.SetActive(true);
            ObjectMovement omc = playerCursor.GetComponentMust<ObjectMovement>();
            cursorPos = omc.position;
            SoundManager.Instance.Play(SoundManager.Instance.level, Sound.Bgm);
            popupUI.restartLvButton_.UnLock();
            popupUI.returnToWorldButton_.UnLock();
        }

        StageManager.Instance.LoadStage();

        if(loadType == LoadType.ToWorld)
        {
            bg_World.SetActive(true);
            bg_Level.SetActive(false);
            playerCursor = objectController.CreateObject(31, cursorPos.x, cursorPos.y);
            SoundManager.Instance.Play(SoundManager.Instance.world, Sound.Bgm);
            
            popupUI.restartLvButton_.Lock();
            popupUI.returnToWorldButton_.Lock();
        }

        // ruleMakingSystem.Init();
    }
}
