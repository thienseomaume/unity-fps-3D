using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertResult : Node
{
    public InvertResult(Node child)
    {
        children = new List<Node>();
        children.Add(child);
    }
    public override void Init(BlackBoard blackBoard)
    {
        base.Init(blackBoard);
        children[0].Init(blackBoard);
    }
    public override NodeStatus Excute()
    {
        NodeStatus result = children[0].Excute();
        if(result == NodeStatus.SUCCESS)
        {
            Debug.Log("failure");
            return NodeStatus.FAILURE;
        }else if(result == NodeStatus.FAILURE)
        {
            Debug.Log("success");
            return NodeStatus.SUCCESS;
        }
        else
        {
            return NodeStatus.RUNNING;
        }
    }
    public override void Exit()
    {
        children[0].Exit();
    }

}
