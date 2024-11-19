using System;
using UnityEngine;

/// <summary>
/// <br>�����̃M�~�b�N�����Z�b�g���邽�߂̎���</br>
/// <br>������P����\��</br>
/// </summary>
public class GimmickResetManager : MonoBehaviour, IInteractable
{
    public Action _resetAction;

    public bool CanInteract()
    {
        return true;
    }

    public string GetInteractionMessage()
    {
        return "�M�~�b�N�����Z�b�g";
    }

    public void OnInteract(IInteractCallBackReceivable caller)
    {
        _resetAction?.Invoke();
    }
}
