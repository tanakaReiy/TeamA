using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTest : MonoBehaviour
{
    private bool _hasProcessed = false;
    private float _rayDistance = 1f;
    private float _switchWidth = 1f; //スイッチの半径
    private float _switchHeight = 0.5f; //高さの半径
    private float _switchDepth = 1f; //奥行き
    private float _switchDistance = 2f;
    private Vector3 _halfExtents;
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
        _halfExtents = new Vector3(_switchWidth, _switchHeight, _switchDepth);
        return Physics.BoxCast(transform.position + new Vector3(0, -0.5f, 0), _halfExtents, Vector3.up, out RaycastHit hit, Quaternion.identity, _switchDistance);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + Vector3.up, _halfExtents + new Vector3(0, _switchDistance, 0));
    }
}
