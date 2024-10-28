using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using System;
using System.Threading;
using UnityEngine;

[Serializable]
/// <summary>一か所を守るやつ</summary>
public sealed class GuardEnemy : IMovePatternEnemy
{
    [SerializeField] private Vector3 _guardPosition;
    [SerializeField] private Vector3 _guardFrontAngle;
    public (Vector3 position, Vector3 direction) NextTarget()
    {
        return (_guardPosition, _guardFrontAngle);
    }

    public async UniTask NextTargetActionAsync(Quaternion targetRotation, Transform transform, CancellationToken token)
    {
        try
        {
            //防衛の向きに合わせて移動する
            await LMotion.Create(transform.rotation, targetRotation, 1).WithEase(Ease.InOutCubic).BindToLocalRotation(transform).ToUniTask(token);
            //最初に少し傾く
            await LMotion.Create(transform.rotation, targetRotation * Quaternion.AngleAxis(-30, Vector3.up), 1.5f).WithEase(Ease.InOutCubic).BindToLocalRotation(transform).ToUniTask(token);
            //ループで左右に回転して警戒をする
            await LMotion.Create(targetRotation * Quaternion.AngleAxis(-30, Vector3.up), targetRotation * Quaternion.AngleAxis(60, Vector3.up), 3f).WithEase(Ease.InOutCubic).WithLoops(-1, loopType: LoopType.Yoyo).BindToLocalRotation(transform).ToUniTask(token);
        }
        catch (OperationCanceledException e)
        {
            Debug.Log("Cancel NextTargetAction");
        }
    }
    /*
#if UNITY_EDITOR
    /// <summary>
    /// 
    /// ※この仮想クラス以外で呼び出さないこと
    /// </summary>
    [Button]
    public void GenerateGuradPosition() 
    {

    }
#endif
    */
}
