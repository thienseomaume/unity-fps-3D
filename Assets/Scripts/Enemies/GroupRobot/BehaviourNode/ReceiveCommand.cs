using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveCommand : Node
{
    string command;
    public ReceiveCommand(string command)
    {
        this.command = command;
    }
    public override NodeStatus Excute()
    {
        Robot owner = blackBoard.owner.GetComponent<Robot>();
        if(owner.group.command == command)
        {
            return NodeStatus.SUCCESS;
        }
        else
        {
            return NodeStatus.FAILURE;
        }
    }
}
