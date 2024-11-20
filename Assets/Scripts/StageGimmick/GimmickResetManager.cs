using System;
using UnityEngine;

/// <summary>
/// <br>複数のギミックをリセットするための実装</br>
/// <br>今後改善する予定</br>
/// </summary>
public class GimmickResetManager : MonoBehaviour, IInteractable, IAbilityDetectable
{
    public Action _resetAction;

    public bool IsEnableDetect => true;

    public bool CanInteract()
    {
        return true;
    }

    public string GetInteractionMessage()
    {
        return "ギミックをリセット";
    }

    public void OnAbilityDetect(WandManager.CaptureAbility ability)
    {
        _resetAction?.Invoke();
    }

    public void OnInteract(IInteractCallBackReceivable caller)
    {
        _resetAction?.Invoke();
    }
}
