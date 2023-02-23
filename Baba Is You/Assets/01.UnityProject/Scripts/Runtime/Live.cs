using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Live : ICommand
{
    [SerializeField]
    private GameObject targetObj_;
    private bool isLive_;
    public int turn_;
    public Live(GameObject target, bool live, int turn)
    {
        targetObj_ = target;
        isLive_ = live;
        turn_ = turn;
    }
    public void Execute()
    {
        targetObj_.SetActive(isLive_);
    }
    public void Undo()
    {
        targetObj_.SetActive(!isLive_);
    }
}
