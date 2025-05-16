using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToTarget : Node
{
    Robot owner;
    public override void Init()
    {
        base.Init();
        owner = blackBoard.owner.GetComponent<Robot>();
    }
    public override NodeStatus Excute()
    {
        Vector3 directionToTarget = (blackBoard.target.position - owner.transform.position).normalized;
        directionToTarget = new Vector3(directionToTarget.x, owner.transform.forward.y, directionToTarget.z).normalized;
        owner.transform.rotation = Quaternion.RotateTowards(owner.transform.rotation, Quaternion.LookRotation(directionToTarget), owner.speedRotate *Time.deltaTime);
        float cosBodyTarget = Vector3.Dot(directionToTarget, owner.transform.forward);
        if (cosBodyTarget >= owner.cosMinToSee)
        {
            return NodeStatus.SUCCESS;
        }
        return NodeStatus.RUNNING;
    }
}
