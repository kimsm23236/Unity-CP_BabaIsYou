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

    public delegate void EventHandler();
    public EventHandler onFadeOut;

    // private Transform[,] gridObjArr = default;

    void Awake()
    {
        debugGridObjectPrefab.SetActive(false);
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
    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x,y) * gridData.cellSize_;
    }
    public void SetupGridData(StageProperty stageProperty)
    {
        gridData = new GridData();
        gridData.width_ = stageProperty.col;
        gridData.height_ = stageProperty.row;
        GFunc.Log($"width : {gridData.width_}, height : {gridData.height_}");
        grid_ = new Grid(gridData.width_, gridData.height_);
        // 일단 그리드 디버그 오브젝트 생성까지 한번에, 나중에 바꿔야됨
        cachedGridObjs = new Transform[gridData.height_, gridData.width_];
        CreateDebugGridText();
    }
}
