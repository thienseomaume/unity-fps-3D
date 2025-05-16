using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotReadyAim : Node
{
    public override NodeStatus Excute()
    {
        if (!blackBoard.alreadyAim)
        {
            return NodeStatus.SUCCESS;
        }
        return NodeStatus.FAILURE;
    }
}
