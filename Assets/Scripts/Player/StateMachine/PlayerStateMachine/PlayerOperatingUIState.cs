using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerOperatingUIState : PlayerBaseState
{
    public PlayerOperatingUIState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }
    IDisposable disposable = null;
    public override void Enter()
    {
        InputReader.Instance.SetEnableInput(false);
        MessageBroker.Default.Publish(new OptionEnable());
        disposable = MessageBroker.Default.Receive<OptionDisable>()
            .Subscribe(_ =>
            {
                ReturnToLocomotion();
            });
        Cursor.lockState = CursorLockMode.None;
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
        InputReader.Instance.SetEnableInput(true);
        disposable?.Dispose();
        Cursor.lockState = CursorLockMode.Locked;

    }


}
