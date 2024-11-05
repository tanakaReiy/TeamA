using Alchemy.Inspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ability;
public class CandleAppearanceChanger : TestCandleGimmick, IInteractable
{
    private bool _canInteract = true;
    private Renderer _candleRenderer;
    [SerializeField] private GameObject _candleObject = null;


    [LabelText("火がついている状態が正しい")]
    [SerializeField] private bool _isFiredCorrect = true;

    /// <summary>
    /// 現在、火がついているかを管理するブール
    /// </summary>
    private bool _isFire = false;

    private void Start()
    {
        _candleRenderer = GetComponent<Renderer>();
        if (_candleObject)
        {
            _candleObject.SetActive(false);
        }
        FindAnyObjectByType<GimmickResetManager>().GetComponent<GimmickResetManager>()._resetAction += ResetGimmick;
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
        if(_canInteract)
        {
            if(_isFire)
            {
                return "火を消す";
            }
            else
            {
                return "火を灯す";
            }
        }
        else
        {
            return "火があれば...";
        }
        return _canInteract ? "火を灯す" : "火があれば...";
    }

    public void OnInteract(IInteractCallBackReceivable caller)
    {
        if (_canInteract)
        {
            _isFire = !_isFire;
            base.ClearActive(_isFiredCorrect ? _isFire : !_isFire);
            if (_candleObject)
            {
                _candleObject.SetActive(_isFire);
            }
            _canInteract = false;
        }
        else
        {
            Debug.Log("何か火があれば……");
        }
    }
    public override void ResetGimmick()
    {
        _isFire = false;
        _candleObject.SetActive(false);
        Debug.Log($"{this.gameObject.name} reset gimmick");
    }
}
