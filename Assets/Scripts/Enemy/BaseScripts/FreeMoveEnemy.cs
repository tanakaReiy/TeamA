using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

//–¢ŽÀ‘•

[Serializable]
/// <summary>Ž©—R‚É“®‚«‰ñ‚é‚â‚Â</summary>
public sealed class FreeMoveEnemy : IMovePatternEnemy
{

    public (Vector3 position, Vector3 direction) NextTarget()
    {
        return (Vector3.zero, Vector3.zero);
    }

    public async UniTask NextTargetActionAsync(Quaternion rotation, Transform transform, CancellationToken token)
    {
        await UniTask.Delay(10);
        Debug.Log("Still this Method Undifined");
    }
}