using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    [SerializeField]
    private List<Move> commandList = new List<Move>();
    
    public int index = 0;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            UndoCommand();
        }
    }
    public void AddCommand(ICommand command)
    {
        commandList.Add(command as Move);
        command.Execute();
    }
    public void UndoCommand()
    {
        if(commandList.Count <= 0)
            return;
        commandList[commandList.Count - 1].Undo();
        commandList.RemoveAt(commandList.Count - 1);
    }
}
