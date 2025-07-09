using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrolling : Node
{
    Robot owner;
    NavMeshAgent agent;
    float maxPatrolX;
    float maxPatrolZ;
    public override void Init(BlackBoard blackBoard)
    {
        base.Init(blackBoard);
        owner = blackBoard.owner.GetComponent<Robot>();
        agent = owner.navMeshAgent;
        maxPatrolX = owner.maxPatrolX;
        maxPatrolZ = owner.maxPatrolZ;
    }
    public override NodeStatus Excute()
    {
        if (owner.HasGroup())
        {
            if (owner.IsGroupLeader() && owner.group.command!=GroupCommand.PATROL)
            {
                owner.group.command = GroupCommand.PATROL;
            }
        }
        if (!owner.AnimCurrentIs(AnimationData.HUMANOID_HOLDING))
        {
            owner.AnimInstant(AnimationData.HUMANOID_HOLDING);
        }
        if (!agent.hasPath)
        {
            Vector3 destination = owner.GetStartPosition() + new Vector3(Random.Range(-maxPatrolX, maxPatrolX), 0, Random.Range(-maxPatrolZ, maxPatrolZ));
            agent.SetDestination(destination);
        }
        else
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.ResetPath();
                return NodeStatus.SUCCESS;
            }
        }
        return NodeStatus.RUNNING;
    }
    public override void Exit()
    {
        agent.ResetPath();
    }
}
