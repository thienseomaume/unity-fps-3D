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
    public override void Init()
    {
        base.Init();
        owner = blackBoard.owner.GetComponent<Robot>();
        agent = owner.navMeshAgent;
        fireCooldown = owner.fireCooldown;
    }
    public override NodeStatus Excute()
    {
        
        agent.ResetPath();
        owner.AnimInstant(AnimationData.HUMANOID_IDLE_FIRE);
        lastTimeFire = Time.time;
        if (owner.IsCurrentAnimStop(AnimationData.HUMANOID_IDLE_FIRE))
        {
            owner.AnimInstant(AnimationData.HUMANOID_IDLE_FIRE);
        }
        if (Time.time >= (lastTimeFire + fireCooldown))
        {
            return NodeStatus.SUCCESS;
        }
        else
        {
            return NodeStatus.RUNNING;
        }
    }
}
