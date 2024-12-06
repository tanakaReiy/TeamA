using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class PlayerBaseState : State
{
    static public readonly string CueSheetName = "CueSheet_0";

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
