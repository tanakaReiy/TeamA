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
    public bool IsFiredCorrect => _isFiredCorrect;

    /// <summary>
    /// 現在、火がついているかを管理するブール
    /// </summary>
    public bool _isFire { get; private set; } = false;

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
        return false;
    }

    public string GetInteractionMessage()
    {
        if (IsEnableDetect)
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
        
    }

    /// <summary>
    /// ロウソクの火が変更された時に呼ぶ関数
    /// </summary>
    /// <param name="newState">火のオンオフ</param>
    public void SetState(bool newState)
    {
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
        //memo ここは複数回変更ができるかどうかで変更が入るかもしれない
        _processed = true;

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
        if (IsEnableDetect && _processed)
        {
            _isFire = !_isFire;
            SetState(_isFiredCorrect ? _isFire : !_isFire);
            if (_candleObject)
            {
                _candleObject.SetActive(_isFire);
                var _candleGimmick = this.GetComponent<TestCandleGimmick>();
                _candleGimmick.OnFire();
                CRIAudioManager.SE.Play3D(Vector3.zero, "CueSheet_0", "SE_fire_tukeru");
            }
            _processed = false;
        }
        else
        {
            Debug.Log("何か火があれば……");
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
