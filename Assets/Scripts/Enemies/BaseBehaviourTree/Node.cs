using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node 
{
    public BlackBoard blackBoard;
    public Node()
    {

    }
    public virtual void Init()
    {

    }
    public abstract NodeStatus Excute();
    public virtual void Exit()
    {

    }
}
