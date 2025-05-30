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
    public virtual void Init()
    {

    }
    public BlackBoard GetBlackBoard()
    {
        return blackBoard;
    }
    public virtual void SetBlackBoard(BlackBoard blackBoard)
    {
        this.blackBoard = blackBoard;
    }
    public abstract NodeStatus Excute();
    public virtual void Exit()
    {

    }
}
