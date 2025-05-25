using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertResult : Node
{
    Node child;
    public InvertResult(Node child)
    {
        this.child = child;
    }
    public override void Init()
    {
        base.Init();
        child.Init();
    }
    public override NodeStatus Excute()
    {
        NodeStatus result = child.Excute();
        if(result == NodeStatus.SUCCESS)
        {
            return NodeStatus.FAILURE;
        }else if(result == NodeStatus.FAILURE)
        {
            return NodeStatus.SUCCESS;
        }
        else
        {
            return NodeStatus.RUNNING;
        }
    }
    public override void SetBlackBoard(BlackBoard blackBoard)
    {
        base.SetBlackBoard(blackBoard);
        child.SetBlackBoard(blackBoard);
    }
    public override void Exit()
    {
        child.Exit();
    }

}
