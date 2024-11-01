using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine _stateMachine = null;

    public PlayerBaseState(PlayerStateMachine playerStateMachine)
    {
        _stateMachine = playerStateMachine;
    }

    protected void ReturnToLocomotion()
    {
        _stateMachine.ChangeState(new PlayerFreeLookState(_stateMachine));
    }
}
