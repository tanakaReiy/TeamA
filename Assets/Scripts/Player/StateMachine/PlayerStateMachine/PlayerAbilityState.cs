using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerBaseState
{
    private readonly int AbilityStateHash = Animator.StringToHash("Ability");
    private const float TransitionDuration = 0.1f;


    public PlayerAbilityState(PlayerStateMachine playerStateMachine) : base(playerStateMachine){}

    public override void Enter()
    {
        _stateMachine.Animator.CrossFadeInFixedTime(AbilityStateHash, TransitionDuration);
        _stateMachine.WandManager.PerformAbility();
    }

    public override void Tick(float deltaTime)
    {
        float time = GetAnimationNormalizedTime(_stateMachine.Animator, "Ability");
        if (time >= 1f )
        {
            ReturnToLocomotion();
        } 
    }

    public override void Exit()
    {
        _stateMachine.WandManager.CancelAbility();
    }

    
}
