using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToLastTargetPos : Node
{
    Robot owner;
    NavMeshAgent agent;
    public override void Init(BlackBoard blackBoard)
    {
        base.Init(blackBoard);
        owner = blackBoard.owner.GetComponent<Robot>();
        agent = owner.navMeshAgent;
    }
    public override NodeStatus Excute()
    {
        Vector3 lastTargetPos;
        if (owner.HasGroup())
        {
            lastTargetPos = owner.group.lastTargetPos;
        }
        else
        {
            lastTargetPos = blackBoard.lastTargetPos;
        }
        if (Vector3.Distance(lastTargetPos, agent.destination) > agent.stoppingDistance)
        {
            agent.SetDestination(lastTargetPos);
        }
        if (Vector3.Distance(agent.nextPosition,agent.destination) <= agent.stoppingDistance)
        {
            if (agent.hasPath)
            {
                agent.ResetPath();
            }
            return NodeStatus.SUCCESS;
        }
        
        if (owner.HasGroup())
        {
            if (owner.IsGroupLeader() && owner.group.command != GroupCommand.MOVE_TO_TARGET)
            {
                owner.group.command = GroupCommand.MOVE_TO_TARGET;
            }
        }
        
        if (!owner.AnimCurrentIs(AnimationData.HUMANOID_AIM))
        {
            owner.AnimInstant(AnimationData.HUMANOID_AIM);
        }
        return NodeStatus.RUNNING;
    }
}
