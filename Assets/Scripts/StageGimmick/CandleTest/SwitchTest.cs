using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTest : MonoBehaviour
{
    private bool _hasProcessed = false;
    private float _rayDistance = 1f;
    private float _switchDistance = 2f;
    [SerializeField] private Vector3 _halfExtents; //スイッチの大きさの半径
    [SerializeField] private LayerMask playerLayer;
    private Vector3 _switchCenterOffset = new Vector3(0, 0.5f, 0);
    public event Action OnSwitchPressed; //ドア開閉通知
    private void FixedUpdate()
    {
        if (_hasProcessed || !gameObject) return;

        if (IsPlayerOnSwitch())
        {
            OnSwitchPressed?.Invoke();
            _hasProcessed = true;
        }
    }
    private bool IsPlayerOnSwitch()
    {
        RaycastHit hit;
        return Physics.BoxCast(transform.position + new Vector3(0, -0.5f, 0)
            , _halfExtents
            , Vector3.up
            , out hit
            , Quaternion.identity
            , _switchDistance, playerLayer.value);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + Vector3.up, _halfExtents + new Vector3(0, _switchDistance, 0));
    }
}
