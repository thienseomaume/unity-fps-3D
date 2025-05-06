using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmStateFire : IState
{
    PlayerController playerController;
    public ArmStateFire(PlayerController playerController)
    {
        this.playerController = playerController;
    }
    public void EnterState()
    {
        playerController.ChangeAnimation(AnimationData.LEFT_CLICK, 0);
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
