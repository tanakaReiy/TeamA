using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;


    public bool IsFalling => _fallingTime > MinFallTimeDelta;
    public float FallingTime => _fallingTime;

    private  float _verticalVelocity;
    private float _fallingTime = 0;
    private Vector3 _impact = Vector3.zero;
    private Vector3 ForceMovement => _impact + Vector3.up * _verticalVelocity;

    private const float DefaultVerticalVelocity = -0.02f;
    private const float MinFallTimeDelta = 0.1f;
    private void Update()
    {
        //重力を適応
        if(_controller.isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = DefaultVerticalVelocity;
            _fallingTime = 0;
        }
        else if(!_controller.isGrounded && _verticalVelocity < 0)
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
    }

    public void Move(float deltaTime) => Move(Vector3.zero, deltaTime);

}
