using Alchemy.Inspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ability;
using System;
public class CandleAppearanceChanger : MonoBehaviour, IInteractable, IResetable, IAbilityDetectable
{
    private bool _processed = true;
    private Renderer _candleRenderer;
    [SerializeField] private GameObject _candleObject = null;
    [LabelText("火がついている状態が正しい")]
    [SerializeField] private bool _isFiredCorrect = true;

    public bool IsFiredCorrect { get; private set; }

    /// <summary>
    /// 現在、火がついているかを管理するブール
    /// </summary>
    private bool _isFire = false;
    public bool IsFire { get; private set; }

    public bool IsEnableDetect => true;

    public event Action OnStateChanged;

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
        //_canInteract = FindAnyObjectByType<PlayerStatus>().GetComponent<PlayerStatus>().Ability is SurtrCaptrable ? true : false;
        //return _canInteract;

        var playerStatus = FindAnyObjectByType<WandManager>();
        var _canInteract = playerStatus == null || (playerStatus.HasAbility() && playerStatus.CurrentAbility != WandManager.CaptureAbility.Test1);
        return _canInteract;
    }

    public string GetInteractionMessage()
    {
        if (CanInteract())
        {
            if (_isFire)
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
    }

    public void OnInteract(IInteractCallBackReceivable caller)
    {
        if (CanInteract() && _processed)
        {
            _isFire = !_isFire;
            SetState(_isFiredCorrect ? _isFire : !_isFire);
            if (_candleObject)
            {
                _candleObject.SetActive(_isFire);
                var _candleGimmick = this.GetComponent<TestCandleGimmick>();
                _candleGimmick.OnFire();
            }
            _processed = false;
        }
        else
        {
            Debug.Log("何か火があれば……");
        }
    }

    /// <summary>
    /// ロウソクの火が変更された時に呼ぶ関数
    /// </summary>
    /// <param name="newState">火のオンオフ</param>
    public void SetState(bool newState)
    {
        _isFire = newState;
        OnStateChanged?.Invoke(); // イベントを発火
    }/// <summary>
     /// リセットアクションの追加
     /// </summary>
    public void RegisterReset()
    {
        try
        {
            FindAnyObjectByType<GimmickResetManager>().GetComponent<GimmickResetManager>()._resetAction += ResetGimmick;
        }
        catch
        {
            Debug.Log($"{this.gameObject.name} can't register ResetGimmick ");
        }
    }
    /// <summary>
    /// ギミックの状態をリセットする
    /// </summary>
    public void ResetGimmick()
    {
        _isFire = false;
        _candleObject.SetActive(false);
        SetState(_isFiredCorrect ? _isFire : !_isFire);
        Debug.Log($"{this.gameObject.name} reset gimmick");
    }

    /// <summary>
    /// 登録したリセットアクションの解除
    /// </summary>
    public void CancelletionReset()
    {
        try
        {
            FindAnyObjectByType<GimmickResetManager>().GetComponent<GimmickResetManager>()._resetAction -= ResetGimmick;
        }
        catch
        {
            Debug.Log($"{this.gameObject.name} can't register ResetGimmick ");
        }
    }
    private void OnDisable()
    {
        CancelletionReset();
    }

    public void OnAbilityDetect(WandManager.CaptureAbility ability)
    {
        if (WandManager.CaptureAbility.Test1 != ability) { return; }

    }
}
