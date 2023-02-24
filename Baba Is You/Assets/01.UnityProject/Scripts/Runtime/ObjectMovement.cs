using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    public Transform movePoint = default;
    public ObjectProperty objectProperty = default;
    private GridPosition position_ = default;
    public GridPosition position
    {
        get
        {
            return position_;
        }
        set
        {
            position_ = value;
        }
    }
    private Direction direction_ = Direction.Right;
    public Direction direction
    {
        get
        {
            return direction_;
        }
        set
        {
            direction_ = value;
        }
    }

    public delegate void EventHandler();
    public EventHandler onMoved;

    void Awake()
    {

    }
    void Start()
    {
        objectProperty = gameObject.GetComponentMust<ObjectProperty>();
        onMoved = new EventHandler(MoveLog);
    }
    void Update()
    {
        Move();
    }
    
    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, 2000f * Time.deltaTime);
    }
    void MoveLog()
    {
        GFunc.Log($"{objectProperty.name} position : {position.x}, {position.y}");
    }
}
