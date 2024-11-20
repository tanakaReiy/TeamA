using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// このインターフェイスを継承するMonoBehaviorクラスはアビリティの影響を受けられるようになります
/// 影響を起こしたAbilityの種類はOnAbilityDetectの引数で判断してください
/// 検出のためにコライダーは必要です
/// </summary>
public interface IAbilityDetectable : IDetectable
{
    void OnAbilityDetect(WandManager.CaptureAbility ability);
}
