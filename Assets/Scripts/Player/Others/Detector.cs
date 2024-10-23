using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Cysharp.Threading.Tasks.Triggers;

[AddComponentMenu("")]
public abstract class Detector<DetectType> : MonoBehaviour where DetectType : IDetectable
{
    private float _detectionInterval = 0.1f;

    public event Action<DetectType> OnDetectMessage;


    public enum DetectMethod
    {
        SendMessageOnce,//OnEnable - OnDisableで検知した場合、一度だけ何かしらのメッセージを飛ばす
        SendMessageAlways,//検知する度にメッセージを飛ばす
        DetectionOnly,//範囲内で検知した最初の参照を残す

    }

    public void EnbleDectection()
    {
        Observable.Timer(TimeSpan.FromSeconds(_detectionInterval))
            .AsUnitObservable()
            .Subscribe(_ => { Detect(); })
            ;
    }

    
    private void Detect()
    {
        
    }



}

public interface IDetectMethod
{
    void Detect(Transform baseTransform);

    void DrawRange(Transform baseTransform);
}

[System.Serializable]
public class Detection : IDetectMethod
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _radius;
    public void Detect(Transform baseTransform)
    {
        
    }

    public void DrawRange(Transform baseTransform)
    {
        Gizmos.DrawWireSphere(baseTransform.TransformPoint(_offset), _radius);
    }
}

public interface IDetectable
{
    //検知されるか
    bool IsDetectable { get; }

    bool OnDetected();

    
}