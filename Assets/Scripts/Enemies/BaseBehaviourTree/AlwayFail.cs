using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwayFail : Node
{
    public AlwayFail(Node child)
    {
        children = new List<Node>();
        children.Add(child);
    }
    public override NodeStatus Excute()
    {
        children[0].Excute();
        return NodeStatus.FAILURE;
    }
}
