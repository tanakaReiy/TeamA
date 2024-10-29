using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System;

public class Detector<DetectType>  where DetectType : class, IDetectable
{
    /// <summary>
    /// 有効なDetectTypeを検知したときに呼ばれます
    /// </summary>
    public IObservable<DetectType> OnDetectAny => _onDetectAny;

    /// <summary>
    /// 衝突履歴
    /// </summary>
    private List<DetectType> _alreadyCollideWih = new();
    
    /// <summary>
    /// OnDetectAnyのSubject
    /// </summary>
    private Subject<DetectType> _onDetectAny = new();

    /// <summary>
    /// 衝突履歴を削除します。衝突履歴はDetectMessageOnceでのみ使用されています
    /// </summary>
    public void ResetColiideInfo() => _alreadyCollideWih.Clear();

    /// <summary>
    /// 範囲内で初めに検知したDetectTypeのみを返します。一度発見したらそれ以降の検知はありません
    /// </summary>
    /// <returns>検知したオブジェクト</returns>
    public DetectType DetectOne(Vector3 start,Vector3 end,float radius,LayerMask mask)
    {
        Vector3 direction = (end - start);
        var hits = Physics.SphereCastAll(start, radius, direction, direction.magnitude, mask.value);

        foreach(RaycastHit hit in hits)
        {
            if(hit.collider.gameObject.TryGetComponent(out DetectType detect))
            {
                if (!detect.IsEnableDetect) { continue; }
                _onDetectAny.OnNext(detect);
                return detect;
            }
        }
        return null;
    }
    public DetectType DetectOne(DetectInfo info)
    {
        return DetectOne(info.start, info.end, info.radius, info.mask);
    }

    /// <summary>
    /// 範囲内にある有効なDetectType全てにメッセージを送ります
    /// </summary>
    public void DetectMessageAlways(Vector3 start, Vector3 end, float radius, LayerMask mask,Action<DetectType> message)
    {
        Vector3 direction = (end - start);
        var hits = Physics.SphereCastAll(start, radius, direction, direction.magnitude, mask.value);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.TryGetComponent(out DetectType detect))
            {
                if (!detect.IsEnableDetect) { continue; }
                message(detect);
                _onDetectAny.OnNext(detect);
            }
        }
        
    }

    public void DetectMessageAlways(DetectInfo info, Action<DetectType> message)
    {
        DetectMessageAlways(info.start, info.end, info.radius, info.mask, message);
    }

    /// <summary>
    /// 範囲内にある有効なDetectTypeで衝突履歴にないオブジェクトにメッセージを送ります
    /// </summary>
    public void DetectMessageOnce(Vector3 start, Vector3 end, float radius, LayerMask mask, Action<DetectType> message)
    {
        Vector3 direction = (end - start);
        var hits = Physics.SphereCastAll(start, radius, direction, direction.magnitude, mask.value);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.TryGetComponent(out DetectType detect))
            {
                if (!detect.IsEnableDetect) { continue; }
                if (_alreadyCollideWih.Contains(detect)) { continue; }
                _alreadyCollideWih.Add(detect);
                message(detect);
                _onDetectAny.OnNext(detect);
            }
        }

    }

    public void DetectMessageOnce(DetectInfo info, Action<DetectType> message)
    {
        DetectMessageOnce(info.start, info.end, info.radius, info.mask,message);
    }


}
[System.Serializable]
public class DetectInfo
{
    public Vector3 start;
    public Vector3 end;
    public float radius;
    public LayerMask mask;
}

public interface IDetectable
{
    bool IsEnableDetect { get; }

}