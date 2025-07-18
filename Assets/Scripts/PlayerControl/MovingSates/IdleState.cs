
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IdleState : IState
{
    PlayerController playerController;
    public IdleState(PlayerController playercontroller)
    {
        this.playerController = playercontroller;
    }
    public void EnterState()
    {
    }

    

    public void ExitState()
    {
        
    }

    public void UpdateState()
    {
    }
    public void FixedUpdateState()
    {
        
    }
}