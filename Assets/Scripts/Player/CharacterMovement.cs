using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    public bool IsEnableManualForce => _followTarget == null;



    IDisposable _targetDisposable = null;
    private Transform _followTarget;

    public bool IsFalling => _fallingTime > MinFallTimeDelta;
    public float FallingTime => _fallingTime;

    private  float _verticalVelocity;
    private float _fallingTime = 0;
    private Vector3 _impact = Vector3.zero;
    private Vector3 ForceMovement => _impact + Vector3.up * _verticalVelocity;

    private bool _isEnableManualForce = true;
    private const float DefaultVerticalVelocity = -0.02f;
    private const float MinFallTimeDelta = 0.1f;


    private void Update()
    {
        //重力を適応
        if (_controller.isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = DefaultVerticalVelocity;
            _fallingTime = 0;
        }
        else if (!_controller.isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity += Physics.gravity.y * Time.deltaTime;
            _fallingTime += Time.deltaTime;
        }
        else
        {
            _verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
        



    }

    //Moveを毎フレーム呼ぶことで重力などが適用されます


    public void Move(Vector3 direction,float deltaTime)
    {
        _controller.Move((direction + ForceMovement) * deltaTime);
        return;
        if (IsEnableManualForce)
        {
        }
        else
        {
            _controller.Move(direction  * deltaTime);

        }
    }

    public void Move(float deltaTime) => Move(Vector3.zero, deltaTime);


    public void SetFollowTarget(Transform followTarget)
    {
        print($"follow target {followTarget}");
        if (followTarget == null)
        {
            _followTarget = null;
            _targetDisposable?.Dispose();
            _targetDisposable = null;
            return;
        }

        _followTarget = followTarget;
        _targetDisposable = followTarget.ObserveEveryValueChanged(t => t.position)
            .Pairwise()
            .Subscribe(pair =>
            {
                var diff = pair.Current - pair.Previous;
                _controller.Move(diff);
            });
    }
}
