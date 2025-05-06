using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.PlayerControl
{
    public class PlayerStateMachine
    {
        public IState currentState { get; set; }
        public void ChangeState(IState state)
        {
            if (state == null) return;
            if(currentState!=null && currentState != state)
            {
                currentState.ExitState();
                currentState = state;
                currentState.EnterState();
            }
        }
        public void UpdateCurrentState()
        {
            if (currentState == null) return;
            currentState.UpdateState();
        }

        public void FixedUpdeateCurrentState()
        {
            if(currentState == null) return;
            currentState.FixedUpdateState();
        }
    }
}
