using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTest : MonoBehaviour
{
    private bool _hasProcessed = false;
    private float _rayDistance = 1f;
    public event Action OnSwitchPressed; //ドア開閉通知
    private void Update()
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
        Ray ray = new Ray(transform.position + new Vector3(0, transform.localScale.y / 2, 0), Vector3.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _rayDistance))
        {
            return true;
        }
        return false;
    }
}
