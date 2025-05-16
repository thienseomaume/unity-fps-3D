using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Formation : ScriptableObject
{
    protected Vector3 AdjustPosition(Vector3 leaderPosition, Vector3 position)
    {
        if(NavMesh.Raycast(leaderPosition,position,out NavMeshHit hit, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return position;
    }
    public abstract Vector3 GetPosition(Group group, Robot robot);
}
