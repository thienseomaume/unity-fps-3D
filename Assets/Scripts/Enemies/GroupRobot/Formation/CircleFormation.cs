using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleFormation : Formation
{
    [SerializeField] float radius;
    public override Vector3 GetPosition(Group group, Robot robot)
    {
        if (group.IsLeader(robot))
        {
            return robot.position;
        }
        Robot leader = group.GetLeader();
        float angle = 360.0f / (group.MembersCount()-1);
        Vector3 position = (Quaternion.AngleAxis(angle * (group.IndexInGroup(robot) - 1), Vector3.up) * Vector3.forward).normalized * radius + leader.position;
        position = AdjustPosition(leader.position, position);
        return position;
    }
}
