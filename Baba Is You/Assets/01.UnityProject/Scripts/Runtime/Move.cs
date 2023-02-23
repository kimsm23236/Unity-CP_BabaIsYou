using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Move : ICommand
{
    [SerializeField]
    private Direction direction = Direction.NONE;
    private GridPosition position;
    private Transform objectMovePoint;
    private Transform[,] targetMovePoint;
    public int turn;

    public Move(Direction direction, GridPosition position, Transform objMovePoint, Transform[,] targets, int turn)
    {
        this.direction = direction;
        this.position = position;
        this.objectMovePoint = objMovePoint;
        this.targetMovePoint = targets;
        this.turn = turn;
    }
    public void Execute()
    {
        GridPosition nextPos = new GridPosition();
        switch(direction)
        {
            case Direction.Right:
            nextPos.x = position.x + 1;
            nextPos.y = position.y;
            break;
            case Direction.Left:
            nextPos.x = position.x - 1;
            nextPos.y = position.y;
            break;
            case Direction.Up:
            nextPos.x = position.x;
            nextPos.y = position.y - 1;
            break;
            case Direction.Down:
            nextPos.x = position.x;
            nextPos.y = position.y + 1;
            break;
        }
        objectMovePoint.transform.position = targetMovePoint[nextPos.y, nextPos.x].position;
        position.x = nextPos.x;
        position.y = nextPos.y;
    }
    public void Undo()
    {
        GridPosition nextPos = new GridPosition();
        switch(direction)
        {
            case Direction.Right:
            nextPos.x = position.x - 1;
            nextPos.y = position.y;
            break;
            case Direction.Left:
            nextPos.x = position.x + 1;
            nextPos.y = position.y;
            break;
            case Direction.Up:
            nextPos.x = position.x;
            nextPos.y = position.y + 1;
            break;
            case Direction.Down:
            nextPos.x = position.x;
            nextPos.y = position.y - 1;
            break;
        }
        objectMovePoint.transform.position = targetMovePoint[nextPos.y, nextPos.x].position;
        position.x = nextPos.x;
        position.y = nextPos.y;
    }
}
