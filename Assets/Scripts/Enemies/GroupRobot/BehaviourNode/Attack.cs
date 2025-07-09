using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attack : Node
{
    float lastTimeFire;
    Robot owner;
    NavMeshAgent agent;
    float fireCooldown;
    public override void Init(BlackBoard blackBoard)
    {
        base.Init(blackBoard);
        owner = blackBoard.owner.GetComponent<Robot>();
        agent = owner.navMeshAgent;
        fireCooldown = owner.fireCooldown;
    }
    public override NodeStatus Excute()
    {
        if (owner.HasGroup())
        {
            owner.group.command = GroupCommand.ATTACK;
        }
        agent.ResetPath();
        if (Time.time >= (lastTimeFire + fireCooldown))
        {
            owner.AnimInstant(AnimationData.HUMANOID_IDLE_FIRE);
            owner.Attack(blackBoard.target);
            lastTimeFire = Time.time;
            return NodeStatus.SUCCESS;
        }
        else
        {
            if (owner.AnimCurrentIs(AnimationData.HUMANOID_IDLE_FIRE) && owner.IsCurrentAnimStop())
            {
                owner.AnimInstant(AnimationData.HUMANOID_AIM);
            }
            return NodeStatus.RUNNING;
        }
    }
    public override void Exit()
    {
        owner.AnimInstant(AnimationData.HUMANOID_AIM);
    }
}
