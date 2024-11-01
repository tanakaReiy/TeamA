using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimaitonEventReceivable;
public class PlayerCapturingState : PlayerBaseState,ICaptureAnimationEventReceivable
{
    private readonly int CaputureStateHash = Animator.StringToHash("Capture");
    private const float TransitionDuration = 0.1f;
    private const string StaffHeadAttachSocketName = "StaffHead";
    public PlayerCapturingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine){}

    public override void Enter()
    {
        _stateMachine.Animator.CrossFadeInFixedTime(CaputureStateHash, TransitionDuration);



    }

    public override void Tick(float deltaTime)
    {
        if(GetAnimationNormalizedTime(_stateMachine.Animator) >= 1f)
        {
            ReturnToLocomotion();
        }
    }

    public override void Exit()
    {
        DisableDetection();
    }

    public void EnableDetection()
    {

        _stateMachine.CapturableDetector.StartDetection(
            capture =>
            {
                _stateMachine.Status.Ability = capture.CapturableAbility;

                GameObject staffHead = GameObject.Instantiate(capture.StaffHeadPrefab);
                IAttachable staffAttachable = staffHead.GetComponent<IAttachable>();
                _stateMachine.SocketManager.AttachTo(staffAttachable, StaffHeadAttachSocketName);

                _stateMachine.CapturableDetector.EndDetection();
            });
    }

    public void DisableDetection()
    {
        _stateMachine.CapturableDetector.EndDetection();
    }
}
