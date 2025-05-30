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
    public override void Init()
    {
        base.Init();
        owner = blackBoard.owner.GetComponent<Robot>();
        agent = owner.navMeshAgent;
        maxPatrolX = owner.maxPatrolX;
        maxPatrolZ = owner.maxPatrolZ;
    }
    public override NodeStatus Excute()
    {
        Debug.Log("patrolling");
        if (owner.HasGroup())
        {
            if (owner.IsGroupLeader() && owner.group.command!="patrol")
            {
                owner.group.command = "patrol";
            }
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
}
