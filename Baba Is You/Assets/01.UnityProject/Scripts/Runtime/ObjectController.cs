using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    private GameObject gameObjPrefab = default;
    private List<GameObject> objectPool = default;
    public List<GameObject> Pool
    {
        get
        {
            return objectPool;
        }
    }
    private GridController gridController = default;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameObjs = GFunc.GetRootObj("GameObjs");
        gridController = gameObjs.FindChildObj("Grid").GetComponentMust<GridController>();
        gameObjPrefab = gameObject.FindChildObj("Object");
        gameObjPrefab.SetActive(false);
        objectPool = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // { stage load process
    public void StageLoad(StageProperty stageProperty)
    {
        CreateObject(stageProperty);
        SetupObjectData();
    }
    public void CreateObject(StageProperty stageProperty)
    {
        string[,] objsStrArr = stageProperty.objs;
        for(int y = 0; y < stageProperty.row; y++)
        {
            for(int x = 0; x < stageProperty.col; x++)
            {
                int id = default;
                bool isSuccess = int.TryParse(objsStrArr[y,x], out id);
                if(id >= 0)
                {
                    GameObject newObj = Instantiate(gameObjPrefab, transform);
                    newObj.transform.SetParent(gameObject.transform);
                    newObj.SetActive(true);
                    ObjectProperty opc = newObj.GetComponentMust<ObjectProperty>();
                    GridPosition pos = new GridPosition();
                    pos.x = x;
                    pos.y = y;
                    opc.id = id;
                    opc.position = pos;
                    objectPool.Add(newObj);
                }
            }
        }
    }
    public void SetupObjectData()
    {
        // 오브젝트 데이터 셋업
        foreach(GameObject obj in objectPool)
        {
            ObjectProperty op = obj.GetComponentMust<ObjectProperty>();
            int x = op.position.x;
            int y = op.position.y;
            op.InitObject();
            obj.SetLocalPos(gridController.gridObjs[y,x].transform.localPosition);
        }

    }
    // } stage load process
}
