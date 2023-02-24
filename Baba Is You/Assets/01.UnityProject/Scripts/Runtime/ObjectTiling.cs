using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTiling : MonoBehaviour
{
    private bool isTilingObject = default;
    private Animator ownAnimator = default;
    private ObjectProperty objectProperty = default;
    private ObjectMovement objectMovement = default;
    private GridPosition position
    {
        get
        {
            return objectMovement.position;
        }
    }
    private ObjectController objectController = default;

    private int width_ = default;
    private int height_ = default;
    private int[] dx = {0, -1, 0, 1};
    private int[] dy = {-1, 0, 1, 0};
    private int[] di = {1000, 100, 10, 1};

    // 타일링 변수
    private int animNum = default;

    public delegate void EventHandler();
    public EventHandler onInitObject;
    public EventHandler onTiling;
    //
    // Start is called before the first frame update
    void Awake()
    {
        GameObject gObjs = GFunc.GetRootObj("GameObjs");
        objectController = gObjs.FindChildObj("ObjectController").GetComponentMust<ObjectController>();
        ownAnimator = gameObject.GetComponentMust<Animator>();
        objectProperty = gameObject.GetComponentMust<ObjectProperty>();
        objectMovement = gameObject.GetComponentMust<ObjectMovement>();
        onInitObject = new EventHandler(Init);
    }
    void Start()
    {
        GameObject gObjs = GFunc.GetRootObj("GameObjs");
        width_ = gObjs.FindChildObj("Grid").GetComponentMust<GridController>().gridData.width_;
        height_ = gObjs.FindChildObj("Grid").GetComponentMust<GridController>().gridData.height_;
        // objectMovement.onMoved += Tiling; 
        onTiling = new EventHandler(Tiling);
        Tiling();
    }

    void Init()
    {
        if(GData.OBJ_ID_TILING.Contains(objectProperty.id))
        {
            isTilingObject = true;
            //ownAnimator.SetInteger("id", ownerOPC.id);
        }
        else
        {
            isTilingObject = false;
            this.enabled = false;
            return;
        }

    }

    void CheckTiling()
    {
        if(isTilingObject == false)
            return;
            
        animNum = 0;

        GridPosition curPos = position;
        for(int i = 0; i < 4; i++)
        {
            GridPosition nextPos = new GridPosition();
            nextPos.x = curPos.x + dx[i];
            nextPos.y = curPos.y + dy[i];

            if(nextPos.x < 0 || nextPos.x >= width_ || nextPos.y < 0 || nextPos.y >= height_)
            {
                animNum += di[i];
                continue;
            }

            List<ObjectProperty> nextPosObjs = objectController.GetObjPropByPos(nextPos);
            foreach(ObjectProperty nextObj in nextPosObjs)
            {
                if(objectProperty.id == nextObj.id)
                {
                    animNum += di[i];
                }
            }
        }

    }
    void SetAnimProp()
    {
        ownAnimator.SetInteger("animValue", animNum);
        ownAnimator.SetInteger("id", objectProperty.id);
    }
    void Tiling()
    {
        if(isTilingObject == false)
            return;

        CheckTiling();
        SetAnimProp(); 
    }
}
