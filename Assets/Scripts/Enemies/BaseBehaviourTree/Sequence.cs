using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    int indexRunning = -1;
    public override void Init()
    {
        base.Init();
        if (blackBoard == null)
        {
            Debug.Log("sequence black board null");
        }
        else
        {
            Debug.Log("sequence black board not null");
        }
        foreach (Node child in children)
        {
            child.Init();
        }
    }
    public Sequence(BlackBoard blackBoard,params Node[] childrenNode)
    {
        this.blackBoard = blackBoard;
        children = new List<Node>(childrenNode);
        foreach (Node child in children)
        {
            child.blackBoard = this.blackBoard;
            Debug.Log("check");
            if(child.blackBoard == null)
            {
                Debug.Log("nullllllllllllll");
            }
        }
    }
    public Sequence(params Node[] childrenNode)
    {
        children = new List<Node>(childrenNode);
        foreach (Node child in children)
        {
            child.blackBoard = this.blackBoard;
        }
    }
    public override void SetBlackBoard(BlackBoard blackBoard)
    {
        base.SetBlackBoard(blackBoard);
        foreach(Node child in children)
        {
            child.SetBlackBoard(blackBoard);
        }
    }
    public override NodeStatus Excute()
    {
        for(int i = 0; i < children.Count; i++)
        {
            NodeStatus status = children[i].Excute();
            if(status == NodeStatus.RUNNING)
            {
                if(indexRunning != -1 && i != indexRunning)
                {
                    children[indexRunning].Exit();
                }
                indexRunning = i;
                return status;
            }
            else if(status == NodeStatus.FAILURE)
            {
                if (indexRunning != -1)
                {
                    children[indexRunning].Exit();
                    indexRunning = -1;
                }
                return status;
            }
        }
        if(indexRunning != -1)
        {
            indexRunning = -1;
        }
        return NodeStatus.SUCCESS;
    }
    public override void Exit()
    {
        base.Exit();
        if (indexRunning != -1)
        {
            children[indexRunning].Exit();
        }
    }
}
