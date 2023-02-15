using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tempmove : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public Transform movePoint = default;
    public LayerMask whatStopMovement = default;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameObjs = GFunc.GetRootObj("GameObjs");
        GameObject gridObj = gameObjs.FindChildObj("Grid");
        movePoint.parent = gridObj.transform;    
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                if(!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, whatStopMovement))
                {
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f) * 24f;
                }
            }

            if(Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                if(!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), .2f, whatStopMovement))
                {
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f) * 24f;
                }
            }
        }

    }
}
