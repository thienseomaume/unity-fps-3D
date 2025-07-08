using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderOrSingle : Node
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
        if(owner.HasGroup())
        {
            //Debug.Log("check 2");
        }
        if (group == null || owner.IsGroupLeader())
        {
            return NodeStatus.SUCCESS;
        }
        else
        {
            return NodeStatus.FAILURE;
        }
    }
}
