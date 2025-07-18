using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmStateNoneAction : IState
{
    private IState currentState;
    private PlayerController playerController;
    public ArmStateNoneAction(PlayerController playerController)
    {
        this.playerController = playerController;
    }
    public void EnterState()
    {
        currentState = playerController.movingStateMachine.currentState;
        if (currentState.GetType() == typeof(IdleState))
        {
            playerController.ChangeAnimation(AnimationData.IDLE, 0.2f);
        }
        else if (currentState.GetType() == typeof(WalkState))
        {
            playerController.ChangeAnimation(AnimationData.WALK, 0.2f);
        }
        else if (currentState.GetType() == typeof(RunState))
        {
            playerController.ChangeAnimation(AnimationData.RUN, 0.2f);
        }
        else if (currentState.GetType() == typeof(OnAirState))
        {
            playerController.ChangeAnimation(AnimationData.IDLE, 0.2f);
        }
    }

    public void ExitState()
    {
        
    }

    public void FixedUpdateState()
    {
        
    }

    public void UpdateState()
    {
        if(currentState != playerController.movingStateMachine.currentState)
        {
            currentState = playerController.movingStateMachine.currentState;
            if(currentState.GetType() == typeof(IdleState))
            {
                playerController.ChangeAnimation(AnimationData.IDLE, 0.2f);
            }else if(currentState.GetType() == typeof(WalkState))
            {
                playerController.ChangeAnimation(AnimationData.WALK, 0.2f);
            }
            else if (currentState.GetType() == typeof(RunState))
            {
                playerController.ChangeAnimation(AnimationData.RUN, 0.2f);
            }
            else if (currentState.GetType() == typeof(OnAirState))
            {
                playerController.ChangeAnimation(AnimationData.IDLE, 0.2f);
            }
        }
    }
}
