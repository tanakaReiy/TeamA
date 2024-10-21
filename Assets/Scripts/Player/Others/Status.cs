using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

//仮ステータスベース
public class ObservableStatus : IObservable<Status>
{
    private readonly ReactiveProperty<Status> _status;

    public ObservableStatus(float value,float max)
    {
        _status = new ReactiveProperty<Status>(new Status(value,max));
    }

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

    public Status(float value, float max)
    {
        if(max < value) { throw new System.ArgumentException(); }
        if(value < 0) { throw new System.ArgumentException(); }
        _value = value;
        _max = max;
        _initialValue = value;
    }

    /// <summary>
    /// Maxを超える値はクランプされます
    /// </summary>
    /// <param name="value"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public Status SetStatus(float value,float max)
    {
        value = Mathf.Clamp(value, 0, max);
        return new Status(value, max);
    }

    public (float value, float max) GetStatus() { return (_value, _max); }

    public Status SetInitialValue()
    {
        return new Status(_initialValue, _max);
    }

    

}
