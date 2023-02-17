using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtrYou : Attribute
{
    private float moveSpeed = 5f;
    public GameObject gridObj = default;
    public Transform movePoint = default;
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
        gridObj = gameObj.FindChildObj("Grid");
        movePoint = gObj_.FindChildObj("MovePoint").transform;
        movePoint.parent = gridObj.transform;
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
        }
    }
}
