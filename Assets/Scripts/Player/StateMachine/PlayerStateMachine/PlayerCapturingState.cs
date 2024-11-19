using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimaitonEventReceivable;
public class PlayerCapturingState : PlayerBaseState,ICaptureAnimationEventReceivable,IPlayerAnimationSePlayable
{
    private readonly int CaputureStateHash = Animator.StringToHash("Capture");
    private const float TransitionDuration = 0.1f;
    private readonly string SpoonSoundCueName = "SE_Spoon";

    private bool _isCapturableDetected = false;


    public PlayerCapturingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine){}


    public override void Enter()
    {
        _stateMachine.Animator.CrossFadeInFixedTime(CaputureStateHash, TransitionDuration);

    }

    public override void Tick(float deltaTime)
    {
        if (GetAnimationNormalizedTime(_stateMachine.Animator,"Capture") >= 1f)
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
                _isCapturableDetected = true;
                capture.OnCaptured.Invoke();
                _stateMachine.WandManager.OnCapture(capture.CapturableAbility);

                _stateMachine.CapturableDetector.EndDetection();
            });
    }

    public void DisableDetection()
    {
        _stateMachine.CapturableDetector.EndDetection();
    }

    public void PlaySe(string cueName)
    {
        CRIAudioManager.SE.Play3D(Vector3.zero, CueSheetName, cueName);
    }

    public void PlaySeThroughState(string cueName)
    {
        if (_isCapturableDetected)
        {
            CRIAudioManager.SE.Play3D(Vector3.zero, CueSheetName, cueName);

        }

    }
}
