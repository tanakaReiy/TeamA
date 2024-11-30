using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alchemy.Inspector;
using Alchemy.Serialization;

[AlchemySerialize]
public partial class WandManager : MonoBehaviour
{
    [SerializeField,AlchemySerializeField] private Dictionary<CaptureAbility,(GameObject Head, AnimationClip Clip)> sources;
    [field:SerializeField]public SocketManager SocketManager { get; private set; }
    [field:SerializeField]public Animator PlayerAnimator { get; private set; }

    private const string StaffHeadAttachSocketName = "StaffHead";
    private const string AbilityClipName = "AbilityEmpty";

    private IAbilityExcuter currentExcuter = null;
    public CaptureAbility CurrentAbility { get; private set; }  
    /// <summary>
    /// �L����Abiity�������Ă��邩
    /// </summary>
    /// <returns></returns>
    public bool HasAbility() => currentExcuter != null;
    /// <summary>
    /// �A�r���e�B�����s���܂�
    /// </summary>
    public void PerformAbility()
    {
        currentExcuter?.PerformAbility();
    }
    /// <summary>
    /// �A�r���e�B���L�����Z�����܂�(�I������)
    /// </summary>
    public void CancelAbility()
    {
        currentExcuter?.CancelAbility();
    }

    /// <summary>
    /// Player��Capturable�����m�����ۂɂ�΂�AAbility��ݒ肵�܂�
    /// </summary>
    /// <param name="ability"></param>
    public void OnCapture(CaptureAbility ability)
    {
        if(!sources.TryGetValue(ability,out var source))
        {
            throw new System.Exception("Ability�ɑΉ�����\�[�X���w�肳��Ă��܂���");
        }
        //Head�̍쐬
        GameObject staffHead = GameObject.Instantiate(source.Head);
        IAbilityExcuter excuter = staffHead.GetComponent<IAbilityExcuter>();
        currentExcuter = excuter;
        SocketManager.DetachFrom(StaffHeadAttachSocketName);
        SocketManager.AttachTo(excuter, StaffHeadAttachSocketName);

        //�A�j���[�V�����̏㏑��
        AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(PlayerAnimator.runtimeAnimatorController);
        PlayerAnimator.runtimeAnimatorController = animatorOverrideController;
        List<KeyValuePair<AnimationClip, AnimationClip>> clipOverrides = new(animatorOverrideController.overridesCount);

        animatorOverrideController.GetOverrides(clipOverrides);

        int overrideIndex = clipOverrides.FindIndex(pair => pair.Key.name.Equals(AbilityClipName));
        if(overrideIndex == -1) { throw new System.Exception("Override����A�j���[�V������������܂���ł���"); }
        clipOverrides[overrideIndex] = new KeyValuePair<AnimationClip, AnimationClip>(clipOverrides[overrideIndex].Key,source.Clip);

        animatorOverrideController.ApplyOverrides(clipOverrides);

        PlayerEventHelper.OnCaptureAbility.OnNext(ability);
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
}