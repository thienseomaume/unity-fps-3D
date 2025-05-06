using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmSateReload : IState
{
    PlayerController playerController;
    public ArmSateReload(PlayerController playerController)
    {
        this.playerController = playerController;
    }
    public void EnterState()
    {
        playerController.ChangeAnimation(AnimationData.R_PRESS, 0);
        Debug.Log("checked");
    }

    public void ExitState()
    {
        
    }

    public void FixedUpdateState()
    {
        
    }

    public void UpdateState()
    {
        
    }
}
