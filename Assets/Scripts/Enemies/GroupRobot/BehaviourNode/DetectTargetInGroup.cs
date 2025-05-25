using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectTargetInGroup : Node
{
    Robot owner;
    Group group;
    public override void Init()
    {
        base.Init();
        if(blackBoard == null)
        {
            Debug.Log("detecttargetingroup blackboard null");
        }
        else
        {
            Debug.Log("detecttargetingroup blackboard not null");
        }
        owner = blackBoard.owner.GetComponent<Robot>();
        group = owner.group;
    }
    public override NodeStatus Excute()
    {
        if(group == null)
        {
            return NodeStatus.FAILURE;
        }
        if (group.detectedTarget)
        {
            return NodeStatus.SUCCESS;
        }
        else
        {
            return NodeStatus.FAILURE;
        }
    }
}
