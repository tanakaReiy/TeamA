using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

//未実装

[Serializable]
/// <summary>自由に動き回るやつ</summary>
public sealed class FreeMoveEnemy : IMovePatternEnemy
{

    (Vector3 position, float direction)[] IMovePatternEnemy.GetAllTargets()
    {
        return new (Vector3 position, float direction)[] { (Vector3.zero , 0)};
    }
    public (Vector3 position, float direction) GetNextTarget()
    {
        return (Vector3.zero, 0);
    }
    public void Dispose() { }

    public async UniTask NextTargetActionAsync(Quaternion rotation, Transform transform, CancellationToken token)
    {
        await UniTask.Delay(10);
        Debug.Log("Still this Method Undifined");
    }
}