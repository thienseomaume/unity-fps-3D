using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    int indexRunning = -1;
    public Selector(params Node[] childrenNode)
    {
        children = new List<Node>(childrenNode);
        foreach (Node child in children)
        {
            child.blackBoard = blackBoard;
        }
    }
    public override NodeStatus Excute()
    {
        for (int i = 0; i < children.Count; i++)
        {
            NodeStatus status = children[i].Excute();
            if (status == NodeStatus.RUNNING)
            {
                if (indexRunning != -1 && indexRunning != i)
                {
                    children[indexRunning].Exit();
                }
                indexRunning = i;
                return status;
            }
            else if (status == NodeStatus.SUCCESS)
            {
                if (indexRunning != -1)
                {
                    children[indexRunning].Exit();
                    indexRunning = -1;
                }
                return status;
            }
        }
        indexRunning = -1;
        return NodeStatus.FAILURE;
    }
    public override void Exit()
    {
        base.Exit();
        if(indexRunning != -1)
        {
            children[indexRunning].Exit();
        }
    }
}
