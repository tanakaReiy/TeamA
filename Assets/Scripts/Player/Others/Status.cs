using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

//���X�e�[�^�X�x�[�X
public class ObservableStatus : IObservable<Status>
{
    private readonly ReactiveProperty<Status> _status;

    public ObservableStatus(float value,float max)
    {
        _status = new ReactiveProperty<Status>(new Status(value,max));
    }

    //���݂̒l��ݒ�A�擾���ł��܂�
    public float Value
    {
        get
        {
            return _status.Value.GetStatus().value;
        }

        set
        {
            _status.Value = _status.Value.SetStatus(value, _status.Value.GetStatus().max);
        }
    }

    //�l�̍ő�l�̐ݒ�A�擾���ł��܂�
    public float Max
    {
        get
        {
            return _status.Value.GetStatus().max;
        }

        set
        {
            _status.Value = _status.Value.SetStatus(_status.Value.GetStatus().value, value);
        }
    }

    //�l�������l�ɐݒ肵�܂�
    public void ResetValue()
    {
        _status.Value = _status.Value.SetInitialValue();
    }


    public IDisposable Subscribe(IObserver<Status> observer)
    {
        return _status.Subscribe(observer);
    }
}


public class Status
{
    private readonly float  _value;
    private readonly float _max;
    private readonly float _initialValue;

    //�����l�ƒl�̏����p����Status���쐬���܂�
    //�����l�̃R�s�[�͎w�肪�Ȃ��ꍇ�Avalue���g�p����܂�
    public Status(float value, float max,float? initialValue = null)
    {
        if(max < value) { throw new System.ArgumentException(); }
        if(value < 0) { throw new System.ArgumentException(); }
        _value = value;
        _max = max;
        _initialValue = initialValue ?? value;

        
    }

    /// <summary>
    /// Value�� 0 - max�ɃN�����v����܂�
    /// </summary>
    /// <param name="value"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public Status SetStatus(float value,float max)
    {
        value = Mathf.Clamp(value, 0, max);
        return new Status(value, max,_initialValue);
    }

    public (float value, float max) GetStatus() { return (_value, _max); }

    public Status SetInitialValue()
    {
        return new Status(_initialValue, _max,_initialValue);
    }

    

}
