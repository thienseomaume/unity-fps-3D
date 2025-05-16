using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Aim : Node
{
    Robot owner;
    public override void Init()
    {
        base.Init();
        owner = blackBoard.owner.GetComponent<Robot>();
    }
    public override NodeStatus Excute()
    {
        if (owner.HasGroup())
        {
            if (owner.IsGroupLeader() && owner.group.command != "attack")
            {
                owner.group.command = "attack";
            }
        }
        owner.AnimCrossFade(AnimationData.HUMANOID_AIM, 0.5f);
        if (owner.IsCurrentAnimStop(AnimationData.HUMANOID_AIM))
        {
            blackBoard.alreadyAim = true;
            return NodeStatus.SUCCESS;
        }
        return NodeStatus.RUNNING;
    }
}
