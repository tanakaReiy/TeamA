using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alchemy.Inspector;
using Alchemy.Serialization;

[AlchemySerialize]
public partial class WandManager : MonoBehaviour
{
    [NonSerialized,AlchemySerializeField] private Dictionary<CaptureAbility,(GameObject Head, AnimationClip Clip)> sources;
    [field:SerializeField]public SocketManager SocketManager { get; private set; }
    [field:SerializeField]public Animator PlayerAnimator { get; private set; }

    private const string StaffHeadAttachSocketName = "StaffHead";
    private const string AbilityClipName = "AbilityEmpty";

    private IAbilityExcuter currentExcuter = null;
    public CaptureAbility CurrentAbility { get; private set; }  
    /// <summary>
    /// 有効なAbiityを持っているか
    /// </summary>
    /// <returns></returns>
    public bool HasAbility() => currentExcuter != null;
    /// <summary>
    /// アビリティを実行します
    /// </summary>
    public void PerformAbility()
    {
        currentExcuter?.PerformAbility();
    }
    /// <summary>
    /// アビリティをキャンセルします(終了処理)
    /// </summary>
    public void CancelAbility()
    {
        currentExcuter?.CancelAbility();
    }

    /// <summary>
    /// PlayerがCapturableを検知した際によばれ、Abilityを設定します
    /// </summary>
    /// <param name="ability"></param>
    public void OnCapture(CaptureAbility ability)
    {
        if(!sources.TryGetValue(ability,out var source))
        {
            throw new System.Exception("Abilityに対応するソースが指定されていません");
        }
        //Headの作成
        GameObject staffHead = GameObject.Instantiate(source.Head);
        IAbilityExcuter excuter = staffHead.GetComponent<IAbilityExcuter>();
        currentExcuter = excuter;
        SocketManager.DetachFrom(StaffHeadAttachSocketName);
        SocketManager.AttachTo(excuter, StaffHeadAttachSocketName);

        //アニメーションの上書き
        AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(PlayerAnimator.runtimeAnimatorController);
        PlayerAnimator.runtimeAnimatorController = animatorOverrideController;
        List<KeyValuePair<AnimationClip, AnimationClip>> clipOverrides = new(animatorOverrideController.overridesCount);

        animatorOverrideController.GetOverrides(clipOverrides);

        int overrideIndex = clipOverrides.FindIndex(pair => pair.Key.name.Equals(AbilityClipName));
        if(overrideIndex == -1) { throw new System.Exception("Overrideするアニメーションが見つかりませんでした"); }
        clipOverrides[overrideIndex] = new KeyValuePair<AnimationClip, AnimationClip>(clipOverrides[overrideIndex].Key,source.Clip);

        animatorOverrideController.ApplyOverrides(clipOverrides);
    }

    //アビリティのenumです
    [Flags]
    public enum CaptureAbility
    {
        None = 0,
        Test1 = 1 << 0,
        Test2 = 1 << 1,
        Test3 = 1 << 2,

    }
}