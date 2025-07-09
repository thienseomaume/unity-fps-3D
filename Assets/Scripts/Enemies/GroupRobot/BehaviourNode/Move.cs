using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Move : Node
{
    NavMeshAgent agent;
    Robot owner;
    Group group;
    public override void Init(BlackBoard blackBoard)
    {
        base.Init(blackBoard);
        agent = blackBoard.agent;
        owner = blackBoard.owner.GetComponent<Robot>();
        group = owner.group;
    }
    public override NodeStatus Excute()
    {
        if (group.command == GroupCommand.PATROL && !owner.AnimCurrentIs(AnimationData.HUMANOID_HOLDING))
        {
            owner.AnimInstant(AnimationData.HUMANOID_HOLDING);
        }
        else if (group.command == GroupCommand.MOVE_TO_TARGET && !owner.AnimCurrentIs(AnimationData.HUMANOID_AIM))
        {
            owner.AnimInstant(AnimationData.HUMANOID_AIM);
        }
        agent.SetDestination(group.formation.GetPosition(group,owner));
        if (Vector3.Distance(agent.nextPosition,agent.destination)<= agent.stoppingDistance)
        {
            //Debug.Log("success");
            //agent.ResetPath();
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
