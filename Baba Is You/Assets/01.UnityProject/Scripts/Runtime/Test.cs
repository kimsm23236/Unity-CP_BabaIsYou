using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject debugGridObjectPrefab;

    public int width_ = default;
    public int height_ = default;
    public float cellSize_ = default;
    private Grid grid_ = default;
    public Camera camera = default;

    // private Transform[,] gridObjArr = default;

    void Awake()
    {
        grid_ = new Grid(width_, height_);
        // gridObjArr = new Transform[height_, width_];
        debugGridObjectPrefab.SetActive(false);
    }
    void Start()
    {
        CreateDebugGridText();
    }
    void Update()
    {
    }

    public void CreateDebugGridText()
    {
        for(int x = 0; x < width_; x++)
        {
            for(int y = 0; y < height_; y++)
            {
                GameObject debugObj_ = Instantiate(debugGridObjectPrefab, GetWorldPosition(x, y), Quaternion.identity);
                debugObj_.transform.parent = gameObject.transform;
                Vector3 InitPos = GetWorldPosition(x,y) + new Vector3(cellSize_ * 0.5f, cellSize_* 0.5f, 0f);
                debugObj_.transform.SetLocalPositionAndRotation(InitPos, Quaternion.identity);
                debugObj_.transform.localScale = Vector3.one;
                debugObj_.SetRectSizeDelta(cellSize_);
                // debugObj_.GetRect()
                //GFunc.LogWarning($"x : {debugObj_.GetRect().sizeDelta.x}, y : {debugObj_.GetRect().sizeDelta.y}");
                debugObj_.gameObject.SetActive(true);
            }
        }
    }
    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x,y) * cellSize_;
    }
}
