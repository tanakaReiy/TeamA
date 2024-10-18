using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleAppearanceChanger : TestCandleGimmick, IInteractable
{
    private bool _canInteract = true;
    private Renderer _candleRenderer;
    private void Start()
    {
        _candleRenderer = GetComponent<Renderer>();
    }
    public bool CanInteract()
    {
        return _canInteract;
    }

    public string GetInteractionMessage()
    {
        return "Œ©‚½–Ú•Ï‚¦‚Ü‚·";
    }

    public void OnInteract(IInteractCallBackReceivable caller)
    {
        if (_canInteract)
        {
            base.ClearActive();
            _candleRenderer.material.color = Color.red;
            _canInteract = false;
        }
    }
}
