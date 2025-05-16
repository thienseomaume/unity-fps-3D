using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Searching : Node
{
    public float timer;
    public float rotateProgress = 0f;
    
    Quaternion originRotation;
    Robot owner;
    Transform spine;
    public override void Init()
    {
        base.Init();
        owner = blackBoard.owner.GetComponent<Robot>();
        spine = owner.spine;
        originRotation = spine.rotation;
    }
    public override NodeStatus Excute()
    {
        if (owner.HasGroup())
        {
            if (owner.IsGroupLeader() && owner.group.command != "search")
            {
                owner.group.command = "search";
            }
        }
        rotateProgress = Mathf.Sin(Mathf.Repeat(rotateProgress + owner.smoothRotate *Time.deltaTime,2*Mathf.PI));
        spine.rotation = originRotation * Quaternion.AngleAxis(owner.maxRotateRange * rotateProgress, Vector3.up);
        if(timer <= 0)
        {
            timer = owner.searchingTime;
            return NodeStatus.SUCCESS;
        }
        else
        {
            timer -= Time.deltaTime;
            return NodeStatus.RUNNING;
        }
    }
}
