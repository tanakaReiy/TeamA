using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleAppearanceChanger : TestCandleGimmick, IInteractable
{
    private bool _canInteract = true;
    private Renderer _candleRenderer;
    [SerializeField] private GameObject _candleObject = null;
    private void Start()
    {
        _candleRenderer = GetComponent<Renderer>();
        if (_candleObject)
        {
            _candleObject.SetActive(false);
        }
    }
    public bool CanInteract()
    {
        //プレイヤーが一つしかないから無理やり探してます
        //推奨：別の方法での参照　要修正
        _canInteract = FindAnyObjectByType<PlayerStatus>().GetComponent<PlayerStatus>().Ability is SurtrCaptrable ? true : false;
        return _canInteract;
    }

    public string GetInteractionMessage()
    {
        return "火を灯す";
        /*
        return "見た目変えます";
        */
    }

    public void OnInteract(IInteractCallBackReceivable caller)
    {
        if (_canInteract)
        {
            base.ClearActive();
            if (_candleObject)
            {
                _candleObject.SetActive(true);
            }
            //_candleRenderer.material.color = Color.red;
            _canInteract = false;
        }
        else
        {
            Debug.Log("何か火があれば……");
        }
    }
}
