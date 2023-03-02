using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtrYou : Attribute
{
    private GridController gridController = default;
    private RuleMakingSystem rms = default;
    public LayerMask whatStopMovement = default;
    public AtrYou()
    {
        AttributeData atrData = DataManager.Instance.dicAtrData[0];
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
        rms = GFunc.GetRootObj("RuleMaker").GetComponentMust<RuleMakingSystem>();
    }
    public override void Execute()
    {
        base.Execute();
        KeyInput();
    }
    private void KeyInput()
    {
        if(Vector3.Distance(owner.transform.position, movePoint.position) <= 0.05f)
        {
            Direction nextDirection = Direction.Right;
            int nextX = position_.x;
            int nextY = position_.y;
            if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                nextDirection = Direction.Right;
                nextX = position_.x + 1;
                nextY = position_.y;
            }
            else if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                nextDirection = Direction.Left;
                nextX = position_.x - 1;
                nextY = position_.y;
            }
            else if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                nextDirection = Direction.Up;
                nextX = position_.x;
                nextY = position_.y - 1;
            }
            else if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                nextDirection = Direction.Down;
                nextX = position_.x;
                nextY = position_.y + 1;
            }
            ownerOmc.direction = nextDirection;
            GridPosition nextPos = new GridPosition();
            nextPos.x = nextX;
            nextPos.y = nextY;
            /*
            if(position_ != nextPos)
            {
                rms.onUpdateRule();
            }
            */
            if(MoveCheck(nextX, nextY))
            {
                //position_.x = nextX;
                //position_.y = nextY;
                // send command
                ICommand movement = new Move(nextDirection, position_, movePoint, gridController.gridObjs, objectController.Turn);
                objectController.PushMove(movement);
                ownerOmc.onMoved();
                
                // ownerOmc.AddCommand(movement);
            }
            // movePoint.position = gridController.gridObjs[position_.y, position_.x].transform.position;

            /* Legacy code
            if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                if(!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, whatStopMovement))
                {
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f) * 24f;
                }
            }
            else if(Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                if(!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), .2f, whatStopMovement))
                {
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f) * 24f;
                }
            }
            */
        }
    }
    private bool MoveCheck(int nextX, int nextY)
    {
        bool isAbleToMove = true;

        // 맵 범위 체크
        if(nextX < 0 || nextX >= gridController.gridData.width_)
            isAbleToMove = false;
        if(nextY < 0 || nextY >= gridController.gridData.height_)
            isAbleToMove = false;

        // 오브젝트 체크 stop
        GridPosition pos = new GridPosition();
        pos.x = nextX;
        pos.y = nextY;
        if(position_ == pos)
            return false;

        List<ObjectProperty> collisionObjs = objectController.GetObjPropByPos(pos);

        // 오브젝트 체크 push
        foreach(ObjectProperty objProp in collisionObjs)
        {
            if(objProp.FindAttribute(3))
            {
                AtrPush next = objProp.GetAttribute(3) as AtrPush;
                if(!next.IsPushed(ownerOmc.direction)) // push 속성을 가지고 있으나 밀지 못하는 경우
                {
                    return false;
                }
            }
            else if(objProp.FindAttribute(2))
            {
                GFunc.Log($"{objProp.Name}은 Stop 가지고 있어");
                return false;
            }
        }

        return isAbleToMove;
    }
}
