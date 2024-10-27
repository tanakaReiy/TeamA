using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

//ñ¢é¿ëï

[Serializable]
/// <summary>èÑâÒÇ∑ÇÈÇ‚Ç¬</summary>
public sealed class PatrolEnemy : IMovePatternEnemy
{
    [SerializeField] private Transform[] _patrolPositions;

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
