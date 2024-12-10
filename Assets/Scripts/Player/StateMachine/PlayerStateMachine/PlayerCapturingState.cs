using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimaitonEventReceivable;
public class PlayerCapturingState : PlayerBaseState,IPlayerAnimationSePlayable
{
    private readonly int CaputureStateHash = Animator.StringToHash("Capture");
    private const float TransitionDuration = 0.1f;

    private Capturable _cachedCapturable = null;
    public PlayerCapturingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine){}

    
    public override void Enter()
    {
        _stateMachine.Animator.CrossFadeInFixedTime(CaputureStateHash, TransitionDuration);
        _cachedCapturable = _stateMachine.Capturaing.CurrentTarget;
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
    }

    public void PlaySe(string cueName)
    {
        CRIAudioManager.SE.Play3D(Vector3.zero, CueSheetName, cueName);
    }

    public void PlaySeThroughState(string cueName)
    {
        if(_cachedCapturable == null) { return; }
        if (_stateMachine.Capturaing.IsInRange(_cachedCapturable.gameObject))
        {
            CRIAudioManager.SE.Play3D(Vector3.zero, CueSheetName, cueName);
            _cachedCapturable.OnCaptured.Invoke();
            _stateMachine.WandManager.ChangeAbility(_cachedCapturable.CapturableAbility);
        }
        

    }
}
