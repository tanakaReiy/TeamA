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

    //現在の値を設定、取得ができます
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

    //値の最大値の設定、取得ができます
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

    //値を初期値に設定します
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

    //初期値と値の上限を用いてStatusを作成します
    //初期値のコピーは指定がない場合、valueが使用されます
    public Status(float value, float max,float? initialValue = null)
    {
        if(max < value) { throw new System.ArgumentException(); }
        if(value < 0) { throw new System.ArgumentException(); }
        _value = value;
        _max = max;
        _initialValue = initialValue ?? value;

        
    }

    /// <summary>
    /// Valueは 0 - maxにクランプされます
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
