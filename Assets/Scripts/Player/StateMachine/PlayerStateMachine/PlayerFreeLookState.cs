using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
public class PlayerFreeLookState : PlayerBaseState
{
    private readonly int WalkStateHash = Animator.StringToHash("Walk");
    private readonly int SpeedPramHash = Animator.StringToHash("Speed");
    private const float AnimatorDampTime = 0.1f;
    private const float TransitionDuration = 0.1f;
    private CompositeDisposable disposables = new();
    public PlayerFreeLookState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.Animator.CrossFadeInFixedTime(WalkStateHash, TransitionDuration);


        InputReader.Instance.OnInteractAsObservable()
            .Where(c => c.performed)
            .Subscribe(_ =>
            {
                _stateMachine.InteractDetector.Interact();
            }).AddTo(disposables);

        InputReader.Instance.OnCaptureAsObservable()
            .Where(c => c.performed)
            .Subscribe(_ =>
            {
                _stateMachine.ChangeState(new PlayerCapturingState(_stateMachine));
            }).AddTo(disposables);
    }

    public override void Tick(float deltaTime)
    {

        var direction = CalculateMovement();

        FaceMovement(direction, deltaTime);

        _stateMachine.CharacterMovement.Move(direction  *_stateMachine.FreeLookMovementSpeed, deltaTime);

        UpdateAnimator(deltaTime);
    }

    private void FaceMovement(Vector3 direction,float deltaTime)
    {
        if(direction.magnitude == 0) { return; }

        _stateMachine.transform.rotation =
            Quaternion.Lerp(_stateMachine.transform.rotation
            , Quaternion.LookRotation(direction)
            , _stateMachine.RotationDampTime * deltaTime);
    }

    private Vector3 CalculateMovement()
    {

        Vector3 forward_xz = Camera.main.transform.forward;
        forward_xz.y = 0;
        forward_xz.Normalize();

        Vector3 right_xz = Camera.main.transform.right;
        right_xz.y = 0;
        right_xz.Normalize();



        return (forward_xz * InputReader.Instance.MovementInput.y
                 + right_xz * InputReader.Instance.MovementInput.x)
                    .normalized;


    }

    private void UpdateAnimator(float deltaTime)
    {
        _stateMachine.Animator.SetFloat(
            SpeedPramHash
            , _stateMachine.Controller.velocity.magnitude / _stateMachine.FreeLookMovementSpeed
            , AnimatorDampTime
            , deltaTime);
    }
    public override void Exit()
    {
        disposables.Dispose();
    }

  
}
