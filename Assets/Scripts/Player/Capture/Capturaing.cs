using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Capturaing : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _maskCollideWith;
#if UNITY_EDITOR
    [SerializeField] private bool _drawGizmos = false;
#endif

    public bool AutoDetectionEnable { get; set; } = true;
    public Capturable CurrentTarget => _currentTarget.Value;
    public IObservable<Capturable> OnCurrentTargetChanged => _currentTarget;

    private ReactiveProperty<Capturable> _currentTarget = new(null);

    private Collider[] _lastColliders;
    private void FixedUpdate()
    {
        if (AutoDetectionEnable)
        {
            ManualDetection();
        }
    }

    public void ManualDetection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.TransformPoint(_offset), _radius, _maskCollideWith);
        Capturable detected = null;
        for (int i = 0; i < colliders.Length; i++)
        {
            if(colliders[i].gameObject.TryGetComponent(out Capturable capturable))
            {
                if(detected != null)
                {
                    //正面を優先的にターゲット
                    //Player->TargetとPlayerForwardの角度をθとしたときのcosθが1に近い方が正面
                    float prevCosValue = Vector3.Dot(transform.forward, (detected.transform.position - transform.position).normalized);
                    float currentCosValue = Vector3.Dot(transform.forward, (capturable.transform.position - transform.position).normalized);

                    //1との差が小さい方を採用
                    if((1 - currentCosValue) < (1 - prevCosValue))
                    {
                        detected = capturable;
                    }
                }
                else
                {
                    detected = capturable;
                }
            }
        }
        _currentTarget.Value = detected;
        _lastColliders = colliders;
    }

    public bool IsInRange(GameObject go)
    {
       return Array.Exists(_lastColliders, (collider) => collider.gameObject == go);
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!_drawGizmos) { return; }
        var centerPos = transform.TransformPoint(_offset);
        Gizmos.DrawSphere(centerPos, _radius);
    }
#endif
}
