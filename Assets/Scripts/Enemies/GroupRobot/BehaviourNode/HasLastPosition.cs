using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasLastPosition : Node
{
    Robot owner;
    Group group;
    public override void Init(BlackBoard blackBoard)
    {
        base.Init(blackBoard);
        owner = blackBoard.owner.GetComponent<Robot>();
        group = owner.group;
    }
    public override NodeStatus Excute()
    {
        if(group != null)
        {
            if(group.lastTargetPos != Vector3.zero)
            {
                return NodeStatus.SUCCESS;
            }
            else
            {
                return NodeStatus.FAILURE;
            }
        }
        else
        {
            if(blackBoard.lastTargetPos != Vector3.zero)
            {
                return NodeStatus.SUCCESS;
            }
            else
            {
                return NodeStatus.FAILURE;
            }
        }

    }
}
