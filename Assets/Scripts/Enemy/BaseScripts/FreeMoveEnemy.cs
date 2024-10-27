using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

//未実装

[Serializable]
/// <summary>自由に動き回るやつ</summary>
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