using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Move : Node
{
    NavMeshAgent agent;
    Robot owner;
    Group group;
    public override void Init()
    {
        base.Init();
        agent = blackBoard.agent;
        owner = blackBoard.owner.GetComponent<Robot>();
        group = owner.group;
    }
    public override NodeStatus Excute()
    {
        agent.SetDestination(group.formation.GetPosition(group,owner));
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.ResetPath();
            return NodeStatus.SUCCESS;
        }
        return NodeStatus.RUNNING;
    }
    public override void Exit()
    {
        base.Exit();
        agent.ResetPath();
    }
}
