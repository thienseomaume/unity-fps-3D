using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Searching : Node
{
    private float timer;

    Robot owner;
    public override void Init(BlackBoard blackBoard)
    {
        base.Init(blackBoard);
        owner = blackBoard.owner.GetComponent<Robot>();
    }
    public override NodeStatus Excute()
    {
        if (owner.HasGroup())
        {
            if (owner.IsGroupLeader() && owner.group.command != GroupCommand.SEARCH)
            {
                owner.group.command = GroupCommand.SEARCH;
            }
        }
        if (!owner.AnimCurrentIs(AnimationData.HUMANOID_SEARCHING))
        {
            int randomRotate = Random.Range(1, 3);
            if (randomRotate == 1)
            {
                owner.AnimInstant(AnimationData.HUMANOID_SEARCHING);
            }
            else
            {
                owner.AnimInstant(AnimationData.HUMANOID_SEARCHING, 0.5f);
            }
            timer = owner.searchingTime;
        }

        if (timer <= 0)
        {
            timer = owner.searchingTime;
            if (owner.HasGroup())
            {
                owner.group.lastTargetPos = Vector3.zero;
            }
            blackBoard.lastTargetPos = Vector3.zero;
            return NodeStatus.SUCCESS;
        }
        else
        {
            timer -= Time.deltaTime;
            return NodeStatus.RUNNING;
        }
    }
    public override void Exit()
    {
        base.Exit();
        if (owner.HasGroup())
        {
            owner.group.lastTargetPos = Vector3.zero;
        }
        blackBoard.lastTargetPos = Vector3.zero;
    }
}
