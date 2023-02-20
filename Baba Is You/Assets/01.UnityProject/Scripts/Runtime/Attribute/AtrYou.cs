using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtrYou : Attribute
{
    private float moveSpeed = 1000f;
    public GridController gridController = default;
    public Transform movePoint = default;
    public GridPosition position_ = default;
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
        owner = gObj_;
        GameObject gameObj = GFunc.GetRootObj("GameObjs");
        gridController = gameObj.FindChildObj("Grid").GetComponentMust<GridController>();
        movePoint = gObj_.FindChildObj("MovePoint").transform;
        movePoint.parent = gridController.transform;
        position_ = owner.GetComponentMust<ObjectProperty>().position;
    }
    public override void Execute()
    {
        Move();
    }
    private void Move()
    {
        owner.transform.position = Vector3.MoveTowards(owner.transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        if(Vector3.Distance(owner.transform.position, movePoint.position) <= 0.05f)
        {
            int nextX = position_.x;
            int nextY = position_.y;
            if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                nextX = position_.x + 1;
                nextY = position_.y;
            }
            else if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                nextX = position_.x - 1;
                nextY = position_.y;
            }
            else if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                nextX = position_.x;
                nextY = position_.y - 1;
            }
            else if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                nextX = position_.x;
                nextY = position_.y + 1;
            }
            if(MoveCheck(nextX, nextY))
            {
                position_.x = nextX;
                position_.y = nextY;
            }
            movePoint.position = gridController.gridObjs[position_.y, position_.x].transform.position;

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

        return isAbleToMove;
    }
}
