using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtrPush : Attribute
{
    private GridController gridController = default;
    private ObjectController objectController = default;
    public AtrPush()
    {
        AttributeData atrData = DataManager.Instance.dicAtrData[3];
        property_ = new AttributeProperty();
        property_.id = atrData.id;
        property_.name = atrData.name;
        property_.priority = 0;
    }
    public override void Attached(GameObject gObj_)
    {
        base.Attached(gObj_);
        GameObject gameObj = GFunc.GetRootObj("GameObjs");
        gridController = gameObj.FindChildObj("Grid").GetComponentMust<GridController>();
        objectController = gameObj.FindChildObj("ObjectController").GetComponentMust<ObjectController>();
    }
    public bool IsPushed(Direction dir)
    {
        bool isSuccessPush = true;
        int nextX = position_.x;
        int nextY = position_.y;
        Direction nextDirection = dir;
        // direction 별 다음 좌표 체크
        switch(dir)
        {
            case Direction.Right:
            nextX = nextX + 1;
            break;
            case Direction.Left:
            nextX = nextX - 1;
            break;
            case Direction.Up:
            nextY = nextY - 1;
            break;
            case Direction.Down:
            nextY = nextY + 1;
            break;
            case Direction.NONE:
            default:
            return false;
        }
        // 다음 좌표로 밀수 있는지를 검사
        // 다음 좌표가 맵 범위 밖인지 검사
        if( nextX < 0 || nextX >= gridController.gridData.width_ ||
            nextY < 0 || nextY >= gridController.gridData.height_)
            return false;
        
        // 밀려는 위치에 있는 오브젝트 검사 * 스탑속성 가지고있는지 푸시 속성 가지고 있는지
        GridPosition nextPos = new GridPosition();
        nextPos.x = nextX;
        nextPos.y = nextY;
        List<ObjectProperty> CollisionObjs = objectController.GetObjPropByPos(nextPos);
        foreach(ObjectProperty obj in CollisionObjs)
        {
            if(obj.FindAttribute(3)) // push 속성 가지고 있는 경우
            {
                AtrPush next = obj.GetAttribute(3) as AtrPush;
                if(!next.IsPushed(dir)) // push 속성을 가지고 있으나 밀지 못하는 경우
                {
                    return false;
                }
            }
            else if(obj.FindAttribute(2)) // 밀려는 위치에 있는 오브젝트가 stop 속성 가지고 있는 경우
                return false;
        }

        // 밀수 있다면 위치 변경
        ICommand movement = new Move(nextDirection, position_, movePoint, gridController.gridObjs, objectController.Turn);
        objectController.PushMove(movement);
        // ownerOmc.AddCommand(movement);

        return isSuccessPush;
    }
}
