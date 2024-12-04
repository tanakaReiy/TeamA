using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Alchemy.Inspector;
public abstract class OverlapDetectionSystem<DetectType> : MonoBehaviour where DetectType : class,IDetectable
{
    [SerializeField, FoldoutGroup("DetectionSystem")] protected Vector3 _offset;
    [SerializeField, FoldoutGroup("DetectionSystem")] protected float _radius;
    [SerializeField, FoldoutGroup("DetectionSystem")] protected LayerMask _maskCollideWith;
#if UNITY_EDITOR
    [SerializeField, FoldoutGroup("DetectionSystem")] private bool _drawGizmos = false;
#endif

    public bool AutoDetectionEnable { get; set; } = true;
    public DetectType CurrentTarget => _currentTarget.Value;
    public IObservable<DetectType> OnCurrentTargetChanged => _currentTarget;

    protected ReactiveProperty<DetectType> _currentTarget = new(null);

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
        DetectType detected = null;
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.TryGetComponent(out DetectType capturable))
            {
                if (!capturable.IsEnableDetect) { continue; }

                if (detected != null)
                {
                    //���ʂ�D��I�Ƀ^�[�Q�b�g
                    //Player->Target��PlayerForward�̊p�x���ƂƂ����Ƃ���cos�Ƃ�1�ɋ߂���������
                    float prevCosValue = Vector3.Dot(transform.forward, (detected.GetTransform().position - transform.position).normalized);
                    float currentCosValue = Vector3.Dot(transform.forward, (capturable.GetTransform().position - transform.position).normalized);

                    //1�Ƃ̍��������������̗p
                    if ((1 - currentCosValue) < (1 - prevCosValue))
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
