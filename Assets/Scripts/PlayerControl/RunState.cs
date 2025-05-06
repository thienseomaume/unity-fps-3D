using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


    public class RunState : IState
    {
        PlayerController playerController;
        private float addSpeed;
        private Vector3 direction;
        public RunState(PlayerController playerController)
        {
            this.playerController = playerController;
            addSpeed = playerController.addSpeed;
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
            playerController.directionBeforeJump = direction.normalized;
        }
        public void FixedUpdateState(){
        Vector3 velocity = direction.normalized * (playerController.speed+addSpeed);
        playerController.playerRigidbody.velocity = new Vector3(velocity.x, playerController.limitY ? Mathf.Min(playerController.playerRigidbody.velocity.y, 0) : playerController.playerRigidbody.velocity.y, velocity.z);
    }
}
