using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvoyFormation : Formation
{
    [SerializeField] float maxSpacing;
    public override Vector3 GetPosition(Group group, Robot robot)
    {
        if (group.IsLeader(robot))
        {
            return robot.position;
        }
        Robot leader = group.GetMember(group.IndexInGroup(robot) - 1);
        Vector3 position = robot.position;
        if ((leader.position - robot.position).sqrMagnitude >= maxSpacing * maxSpacing)
        {
            position = -leader.direction * maxSpacing;
        }
        position = AdjustPosition(leader.position, position);
        return position;
    }
}
