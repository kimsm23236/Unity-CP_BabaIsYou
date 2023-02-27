using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    public int width_;
    public int height_;
    public float cellSize_;
    public GridData()
    {
        cellSize_ = 24f;
    }
}
public class GridController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject debugGridObjectPrefab;

    public GridData gridData = default;
    private Grid grid_ = default;
    public Camera camera = default;

    private Transform[,] cachedGridObjs = default;
    public Transform[,] gridObjs
    {
        get
        {
            return cachedGridObjs;
        }
    }

    private bool isNeedInitForEditor = true;

    public delegate void EventHandler();
    public EventHandler onFadeOut;

    // private Transform[,] gridObjArr = default;

    void Awake()
    {
        debugGridObjectPrefab.SetActive(false);
        cachedGridObjs = new Transform[50, 50];
    }
    void Start()
    {
        
    }
    void Update()
    {
    }

    public void CreateDebugGridText()
    {
        int width_ = gridData.width_;
        int height_ = gridData.height_;
        float cellSize_ = gridData.cellSize_;

        for(int x = 0; x < width_; x++)
        {
            for(int y = 0; y < height_; y++)
            {
                GameObject debugObj_ = Instantiate(debugGridObjectPrefab, GetWorldPosition(x, y), Quaternion.identity);
                debugObj_.transform.parent = gameObject.transform;
                Vector3 InitPos = GetWorldPosition(x,-y) + new Vector3(cellSize_ * 0.5f, -cellSize_* 0.5f, 0f);
                debugObj_.transform.SetLocalPositionAndRotation(InitPos, Quaternion.identity);
                debugObj_.transform.localScale = Vector3.one;
                debugObj_.SetRectSizeDelta(cellSize_);
                cachedGridObjs[y,x] = debugObj_.transform;
                // debugObj_.GetRect()
                //GFunc.LogWarning($"x : {debugObj_.GetRect().sizeDelta.x}, y : {debugObj_.GetRect().sizeDelta.y}");
                debugObj_.gameObject.SetActive(true);
            }
        }
    }
    public void DeleteDebugGridText()
    {
        for(int x = 0 ; x < cachedGridObjs.GetLength(0); x++)
        {
            for(int y = 0 ; y < cachedGridObjs.GetLength(1); y++)
            {
                Destroy(cachedGridObjs[y,x], 0.1f);
            }
        }
    }

    public void DeleteDebugGridText_Edit()
    {
        if(cachedGridObjs[0,0] == null || cachedGridObjs[0,0] == default)
        {
            GFunc.Log("cached null");
            return;
        }
        for(int x = 0 ; x < cachedGridObjs.GetLength(0); x++)
        {
            for(int y = 0 ; y < cachedGridObjs.GetLength(1); y++)
            {
                if(cachedGridObjs[y,x] == null || cachedGridObjs[y,x] == default)
                    continue;
                DestroyImmediate(cachedGridObjs[y,x].gameObject, true);
            }
        }
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x,y) * gridData.cellSize_;
    }
    public void SetupGridData(StageProperty stageProperty)
    {
        DeleteDebugGridText();
        gridData = new GridData();
        gridData.width_ = stageProperty.col;
        gridData.height_ = stageProperty.row;
        GFunc.Log($"width : {gridData.width_}, height : {gridData.height_}");
        grid_ = new Grid(gridData.width_, gridData.height_);
        cachedGridObjs = new Transform[gridData.height_, gridData.width_];
        // 일단 그리드 디버그 오브젝트 생성까지 한번에, 나중에 바꿔야됨
        CreateDebugGridText();
    }
    public void Init_Editor()
    {
        debugGridObjectPrefab.SetActive(false);
        if(isNeedInitForEditor)
        {
            isNeedInitForEditor = false;
            cachedGridObjs = new Transform[50, 50];
            GFunc.Log("cachedGridObjs 50 50 Init");
        } 
        GFunc.Log("Init");
    }
    public void SetupGrid_Editor(int width, int height)
    {
        DeleteDebugGridText_Edit();
        gridData = new GridData();
        gridData.width_ = width;
        gridData.height_ = height;
        grid_ = new Grid(gridData.width_, gridData.height_);
        CreateDebugGridText();
    }
}
