using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToLastTargetPos : Node
{
    Robot owner;
    NavMeshAgent agent;
    public override void Init()
    {
        base.Init();
        owner = blackBoard.owner.GetComponent<Robot>();
        agent = owner.navMeshAgent;
    }
    public override NodeStatus Excute()
    {
        if (owner.HasGroup())
        {
            if (owner.IsGroupLeader() && owner.group.command != "move")
            {
                owner.group.command = "move";
            }
        }
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.ResetPath();
            return NodeStatus.SUCCESS;
        }
        return NodeStatus.RUNNING;
    }
}
