using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alchemy.Inspector;
using Alchemy.Serialization;
using UniRx;

[AlchemySerialize]
public partial class WandManager : MonoBehaviour
{
    [SerializeField, FoldoutGroup("DetectionSystem")] private Vector3 _offset;
    [SerializeField, FoldoutGroup("DetectionSystem")] private float _radius;
    [SerializeField, FoldoutGroup("DetectionSystem")] private LayerMask _maskCollideWith;
#if UNITY_EDITOR
    [SerializeField, FoldoutGroup("DetectionSystem")] private bool _drawGizmos = false;
#endif

    [SerializeField] private List<WandHeader> _headers;
    [field:SerializeField]public SocketManager SocketManager { get; private set; }
    [field:SerializeField]public Animator PlayerAnimator { get; private set; }

    private const string StaffHeadAttachSocketName = "StaffHead";
    private const string AbilityClipName = "AbilityEmpty";

    public CaptureAbility CurrentAbility { get; private set; }
    /// <summary>
    /// �L����Abiity�������Ă��邩
    /// </summary>
    /// <returns></returns>
    public bool HasAbility() => CurrentAbility != CaptureAbility.None;


    public bool AutoDetectionEnable { get; set; } = true;
    /// <summary>
    /// PerformAbility�����s�����ۂ�Ability�̎��s�Ώې�
    /// </summary>
    public IAbilityDetectable CurrentTarget => _currentTarget.Value;

    public IObservable<IAbilityDetectable> OnCurrentTargetChanged => _currentTarget;

    protected ReactiveProperty<IAbilityDetectable> _currentTarget = new(null);


    private void FixedUpdate()
    {
        if (AutoDetectionEnable && HasAbility())
        {
            ManualDetection();
        }
    }
    /// <summary>
    /// �A�r���e�B�����s���܂�
    /// </summary>
    public void PerformAbility()
    {
        CurrentTarget?.OnAbilityDetect(CurrentAbility);
    }


    public void ChangeAbility(CaptureAbility ability)
    {
        //Head�̍쐬
        WandHeader headInPrefab = _headers.Find(head => head.Ability == ability);
        if (headInPrefab == null) { throw new Exception("�Ή�����Head�����݂��܂���"); }

        GameObject headObject = Instantiate(headInPrefab.gameObject);
        WandHeader header = headObject.GetComponent<WandHeader>();
        SocketManager.DetachFrom(StaffHeadAttachSocketName);
        SocketManager.AttachTo(header, StaffHeadAttachSocketName);

        //�A�j���[�V�����̏㏑��
        AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(PlayerAnimator.runtimeAnimatorController);
        PlayerAnimator.runtimeAnimatorController = animatorOverrideController;
        List<KeyValuePair<AnimationClip, AnimationClip>> clipOverrides = new(animatorOverrideController.overridesCount);

        animatorOverrideController.GetOverrides(clipOverrides);

        int overrideIndex = clipOverrides.FindIndex(pair => pair.Key.name.Equals(AbilityClipName));
        if (overrideIndex == -1) { throw new System.Exception("Override����A�j���[�V������������܂���ł���"); }
        clipOverrides[overrideIndex] = new KeyValuePair<AnimationClip, AnimationClip>(clipOverrides[overrideIndex].Key, header.OverrideAnimationClip);

        animatorOverrideController.ApplyOverrides(clipOverrides);

        CurrentAbility = ability;

        PlayerEventHelper.OnCaptureAbility.OnNext(ability);
    }
    public void ManualDetection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.TransformPoint(_offset), _radius, _maskCollideWith);
        IAbilityDetectable detected = null;
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.TryGetComponent(out IAbilityDetectable abilityDetectable))
            {
                if (!abilityDetectable.IsEnableDetect) { continue; }
                if (!abilityDetectable.IsAvailable(CurrentAbility)) { continue; }
                if (detected != null)
                {
                    //���ʂ�D��I�Ƀ^�[�Q�b�g
                    //Player->Target��PlayerForward�̊p�x���ƂƂ����Ƃ���cos�Ƃ�1�ɋ߂���������
                    float prevCosValue = Vector3.Dot(transform.forward, (detected.GetTransform().position - transform.position).normalized);
                    float currentCosValue = Vector3.Dot(transform.forward, (abilityDetectable.GetTransform().position - transform.position).normalized);

                    //1�Ƃ̍��������������̗p
                    if ((1 - currentCosValue) < (1 - prevCosValue))
                    {
                        detected = abilityDetectable;
                    }
                }
                else
                {
                    detected = abilityDetectable;
                }
            }
        }
        _currentTarget.Value = detected;
    }

    //�A�r���e�B��enum�ł�
    [Flags]
    public enum CaptureAbility
    {
        None = 0,
        Test1 = 1 << 0,
        Test2 = 1 << 1,
        Test3 = 1 << 2,

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