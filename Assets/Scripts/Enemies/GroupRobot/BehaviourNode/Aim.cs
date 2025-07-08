using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Aim : Node
{
    Robot owner;
    public override void Init(BlackBoard blackBoard)
    {
        base.Init(blackBoard);
        owner = blackBoard.owner.GetComponent<Robot>();
    }
    public override NodeStatus Excute()
    {
        Debug.Log("aim");
        if (owner.HasGroup())
        {
            if (owner.IsGroupLeader() && owner.group.command != GroupCommand.ATTACK)
            {
                owner.group.command = GroupCommand.ATTACK;
            }
        }
        AnimatorStateInfo nextState = owner.GetNextSate();
        if (!owner.AnimCurrentIs(AnimationData.HUMANOID_AIM) && nextState.shortNameHash!= AnimationData.HUMANOID_AIM)
        {
            owner.AnimCrossFade(AnimationData.HUMANOID_AIM,0.5f);
        }
        else
        {
            if (owner.IsCurrentAnimStop())
            {
                blackBoard.alreadyAim = true;
                return NodeStatus.SUCCESS;
            }
        }
        return NodeStatus.RUNNING;
    }
}
