using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System;

public class Detector<DetectType>  where DetectType : class, IDetectable
{
    /// <summary>
    /// �L����DetectType�����m�����Ƃ��ɌĂ΂�܂�
    /// </summary>
    public IObservable<DetectType> OnDetectAny => _onDetectAny;

    /// <summary>
    /// �Փ˗���
    /// </summary>
    private List<DetectType> _alreadyCollideWih = new();
    
    /// <summary>
    /// OnDetectAny��Subject
    /// </summary>
    private Subject<DetectType> _onDetectAny = new();

    /// <summary>
    /// �Փ˗������폜���܂��B�Փ˗�����DetectMessageOnce�ł̂ݎg�p����Ă��܂�
    /// </summary>
    public void ResetColiideHistory() => _alreadyCollideWih.Clear();

   


    /// <summary>
    /// �͈͓��ŏ��߂Ɍ��m����DetectType�݂̂�Ԃ��܂��B��x���������炻��ȍ~�̌��m�͂���܂���
    /// </summary>
    /// <returns>���m�����I�u�W�F�N�g</returns>
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
    /// �͈͓��ɂ���L����DetectType�S�ĂɃ��b�Z�[�W�𑗂�܂�
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
    /// �͈͓��ɂ���L����DetectType�ŏՓ˗����ɂȂ��I�u�W�F�N�g�Ƀ��b�Z�[�W�𑗂�܂�
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
    Transform GetTransform();

}