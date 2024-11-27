using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendantController : MonoBehaviour, IInteractable, IActivatable
{
    public event Action OnActivated;

    public bool CanInteract()
    {
        return true;
    }

    public string GetInteractionMessage()
    {
        return "ドア開けます";
    }

    public void OnInteract(IInteractCallBackReceivable caller)
    {
        Debug.Log("インタラクト確認");
        OnActivated?.Invoke();
    }
}
