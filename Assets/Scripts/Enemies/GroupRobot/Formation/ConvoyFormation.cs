using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConvoyFormation", menuName = "Group Formation/Convoy")]

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
        float distanceToLeader = Vector3.Distance(leader.position, position);
        if (distanceToLeader > maxSpacing)
        {
            position = leader.position + (position - leader.position).normalized * maxSpacing;
            position = AdjustPosition(leader.position, position);
        }
        return position;
    }

}
