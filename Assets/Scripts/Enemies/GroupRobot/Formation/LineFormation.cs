using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LineFormation", menuName = "Group Formation/Line")]
public class LineFormation : Formation
{
    [SerializeField] float spacing;
    public override Vector3 GetPosition(Group group, Robot robot)
    {
        if (group.IsLeader(robot))
        {
            return robot.position;
        }
        Robot leader = group.GetLeader();
        int followerCount = group.FollowerCount();
        int index = group.IndexInGroup(robot)-1;
        Vector3 position;
        Vector3 leftHandLeader = -Vector3.Cross(leader.direction, Vector3.up).normalized;
        Vector3 rightHandLeader = -leftHandLeader;
        if (index % 2 == 1)
        {
            position = ((index + 2 - 1) / 2) * spacing * leftHandLeader - leader.direction * spacing+leader.position;
        }
        else
        {
            position = (index / 2) * spacing * rightHandLeader - leader.direction * spacing + leader.position;
        }
        position = AdjustPosition(leader.position, position);
        return position;
    }
}
