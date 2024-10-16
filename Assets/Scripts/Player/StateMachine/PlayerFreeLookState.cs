using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerFreeLookState : PlayerBaseState
{
    public PlayerFreeLookState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        InputReader.Instance.OnInteractAsObservable()
            .Where(c => c.performed)
            .Subscribe(_ =>
            {
                _stateMachine.InteractDetector.Interact();
            }).AddTo(_stateMachine.gameObject);
    }

    public override void Tick(float deltaTime)
    {
        var direction = CalculateMovement();

        FaceMovement(direction, deltaTime);

        _stateMachine.CharacterMovement.Move(direction  *_stateMachine.FreeLookMovementSpeed, deltaTime);
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
    public override void Exit()
    {

    }

  
}
