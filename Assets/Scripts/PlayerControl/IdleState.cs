
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
        playerController.playerRigidbody.velocity = new Vector3(0, playerController.limitY ? Mathf.Min(playerController.playerRigidbody.velocity.y, 0) : playerController.playerRigidbody.velocity.y, 0);
        //playerController.directionBeforeJump = Vector3.zero;
    }

    

    public void ExitState()
    {
        
    }

    public void UpdateState()
    {
        playerController.playerRigidbody.velocity = new Vector3(0, playerController.limitY ? Mathf.Min(playerController.playerRigidbody.velocity.y, 0) : playerController.playerRigidbody.velocity.y, 0);
    }
    public void FixedUpdateState()
    {
        
    }
}