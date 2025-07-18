using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmSateRPress : IState
{
    PlayerController playerController;
    public ArmSateRPress(PlayerController playerController)
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
