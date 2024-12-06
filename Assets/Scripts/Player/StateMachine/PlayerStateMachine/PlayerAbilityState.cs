using Cysharp.Threading.Tasks;
using LitMotion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerBaseState,AnimaitonEventReceivable.IPlayerAnimationAbilityPerformable
{
    private readonly int AbilityStateHash = Animator.StringToHash("Ability");
    private const float TransitionDuration = 0.1f;
    private bool IsTransitioned = false;
    private const float RotationTime = 0.1f;
    public PlayerAbilityState(PlayerStateMachine playerStateMachine) : base(playerStateMachine){}

    public override void Enter()
    {
        if (!_stateMachine.WandManager.HasAbility()) { ReturnToLocomotion();return; }

        UniTask.Create(async () =>
        {
            if(_stateMachine.WandManager.CurrentTarget != null)
            {
                Vector3 targetDirection =
                _stateMachine.WandManager.CurrentTarget.GetTransform().position - _stateMachine.transform.position;
                targetDirection.y = 0;
                targetDirection.Normalize();

                await LMotion.Create(
                    _stateMachine.transform.rotation,
                    Quaternion.LookRotation(targetDirection),
                    RotationTime)
                .Bind(r => _stateMachine.transform.rotation = r)
                .ToUniTask();
            }
            _stateMachine.Animator.CrossFadeInFixedTime(AbilityStateHash, TransitionDuration);
            IsTransitioned = true;

        }).Forget();

    }

    public override void Tick(float deltaTime)
    {
        if (!IsTransitioned) { return; }
        float time = GetAnimationNormalizedTime(_stateMachine.Animator, "Ability");
        if (time >= 1f )
        {
            ReturnToLocomotion();
        } 
    }

    public override void Exit()
    {
    }

    public void PerformAbility()
    {
        _stateMachine.WandManager.PerformAbility();
    }
}
