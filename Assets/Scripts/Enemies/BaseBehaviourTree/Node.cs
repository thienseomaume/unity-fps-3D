using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node 
{
    public List<Node> children;
    public BlackBoard blackBoard;
    public Node()
    {

    }
    public virtual void Init(BlackBoard blackBoard)
    {
        this.blackBoard = blackBoard;
        if (children != null)
        {
            foreach (Node child in children)
            {
                child.Init(blackBoard);
            }
        }
    }
    public BlackBoard GetBlackBoard()
    {
        return blackBoard;
    }
    public abstract NodeStatus Excute();
    public virtual void Exit()
    {

    }
}
