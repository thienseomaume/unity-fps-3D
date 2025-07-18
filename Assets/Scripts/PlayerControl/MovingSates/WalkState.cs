using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class WalkState : IState
{
    PlayerController playerController;
    private Vector3 direction;
    public WalkState(PlayerController playerController)
    {
        this.playerController = playerController;
    }
    public void EnterState()
    {
    }

    public void ExitState()
    {

    }

    public void UpdateState()
    {
        direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            direction += playerController.transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += -playerController.transform.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += playerController.transform.right;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += -playerController.transform.right;
        }

    }
    public void FixedUpdateState()
    {
        Vector3 force = direction.normalized * playerController.force;
        playerController.playerRigidbody.AddForce(force, ForceMode.Force);
    }
}
