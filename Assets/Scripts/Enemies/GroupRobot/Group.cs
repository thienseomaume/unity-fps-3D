using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{
    public List<Robot> members;
    private List<bool> targetDetect;
    public Formation formation;
    public bool detectedTarget;
    public Vector3 lastTargetPos = Vector3.zero;
    public string command;
    
    private bool UpdateTargetDetect()
    {
        foreach(bool detect in targetDetect)
        {
            if(detect == true)
            {
                return true;
            }
        }
        return false;
    }
    public Robot GetLeader()
    {
        return members[0];
    }
    
    public bool IsLeader(Robot robot)
    {
        if (members.IndexOf(robot) == 0)
        {
            return true;
        }
        return false;
    }
    public Robot GetMember(int index)
    {
        return members[index];
    }
    public void AddMember(Robot robot)
    {
        members.Add(robot);
        robot.group = this;
    }
    public void RemoveMember(Robot robot)
    {
        members.Remove(robot);
        robot.group = null;
    }
    public int IndexInGroup(Robot robot)
    {
        return members.IndexOf(robot);
    }
    public int MembersCount()
    {
        return members.Count;
    }
    public int FollowerCount()
    {
        return MembersCount() - 1;
    }
    private void Awake()
    {
        foreach(Robot robot in members)
        {
            robot.group = this;
        }
        targetDetect = new List<bool>(members.Count);
    }
    private void LateUpdate()
    {
        detectedTarget = UpdateTargetDetect();
    }
}
