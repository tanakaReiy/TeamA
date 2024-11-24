using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

//������

[Serializable]
/// <summary>���R�ɓ��������</summary>
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
    public void OnDisposed() { }

    public async UniTask NextTargetActionAsync(Quaternion rotation, Transform transform, CancellationToken token)
    {
        await UniTask.Delay(10);
        Debug.Log("Still this Method Undifined");
    }
}