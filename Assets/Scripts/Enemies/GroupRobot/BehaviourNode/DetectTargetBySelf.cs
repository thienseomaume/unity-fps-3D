using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectTargetBySelf : Node
{
    Robot owner;
    Transform target;
    public override void Init(BlackBoard blackBoard)
    {
        base.Init(blackBoard);
        target = blackBoard.target;
        owner = blackBoard.owner.GetComponent<Robot>();
    }
    public override NodeStatus Excute()
    {
        if (target == null)
        {
            return NodeStatus.FAILURE;
        }
        Vector3 directionToTarget = target.position - owner.viewPoint.position;
        directionToTarget = new Vector3(directionToTarget.x, 0, directionToTarget.z);
        float cos = Vector3.Dot(owner.viewPoint.up, directionToTarget.normalized);
        if(directionToTarget.magnitude<=owner.maxDetectRange && cos>0 && cos <= Mathf.Cos(owner.halfOfView))
        {
            float distance = directionToTarget.magnitude;
            if (!Physics.Raycast(owner.viewPoint.position,directionToTarget, distance, owner.obstacleLayer))
            {
                blackBoard.lastTargetPos = target.position;
                if (owner.HasGroup())
                {
                    owner.group.UpdateTargetDetectIndividual(owner, true);
                }
                return NodeStatus.SUCCESS;
            }
        }
        
        if (owner.HasGroup() && blackBoard.lastTargetPos != Vector3.zero)
        {
            owner.group.UpdateTargetDetectIndividual(owner, false);
            owner.group.lastTargetPos = blackBoard.lastTargetPos;
            blackBoard.lastTargetPos = Vector3.zero;
        }

        return NodeStatus.FAILURE;
    }
}
