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
    [LabelText("�΂����Ă����Ԃ�������")]
    public bool IsFiredCorrect { get; private set; }  = true;

    /// <summary>
    /// ���݁A�΂����Ă��邩���Ǘ�����u�[��
    /// </summary>
    private bool _isFire = false;

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
                return "�΂�����";
            }
            else
            {
                return "�΂𓔂�";
            }
        }
        else
        {
            return "�΂������...";
        }
    }

    public void OnInteract(IInteractCallBackReceivable caller)
    {
        
    }

    /// <summary>
    /// ���E�\�N�̉΂��ύX���ꂽ���ɌĂԊ֐�
    /// </summary>
    /// <param name="newState">�΂̃I���I�t</param>
    public void SetState(bool newState)
    {
        _isFire = newState;
        OnStateChanged?.Invoke(); // �C�x���g�𔭉�
    }/// <summary>
     /// ���Z�b�g�A�N�V�����̒ǉ�
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
    /// �M�~�b�N�̏�Ԃ����Z�b�g����
    /// </summary>
    public void ResetGimmick()
    {
        //memo �����͕�����ύX���ł��邩�ǂ����ŕύX�����邩������Ȃ�
        _processed = true;

        _isFire = false;
        _candleObject.SetActive(false);
        SetState(IsFiredCorrect ? _isFire : !_isFire);
        Debug.Log($"{this.gameObject.name} reset gimmick");
    }

    /// <summary>
    /// �o�^�������Z�b�g�A�N�V�����̉���
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
            SetState(IsFiredCorrect ? _isFire : !_isFire);
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
            Debug.Log("�����΂�����΁c�c");
        }
    }
}
