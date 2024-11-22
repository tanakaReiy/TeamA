using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

//ñ¢é¿ëï

[Serializable]
/// <summary>èÑâÒÇ∑ÇÈÇ‚Ç¬</summary>
public sealed class PatrolEnemy : IMovePatternEnemy
{
    [Serializable]
    public class PatrolPointData
    {
        public Vector3 Position;
        public Vector3 Direction;
    }


    [SerializeField] PatrolPointData[] patrolPositions;

    Vector3[] IMovePatternEnemy.GetAllTargets()
    {
        Vector3[] positions = new Vector3[patrolPositions.Length];
        for (int i= 0; i < patrolPositions.Length; i++)
        {
            positions[i] = patrolPositions[i].Position;
        }
        return positions;
    }
    public (Vector3 position, Vector3 direction) GetNextTarget()
    {
        return (Vector3.zero, Vector3.zero);
    }

    public async UniTask NextTargetActionAsync(Quaternion rotation, Transform transform, CancellationToken token)
    {
        await UniTask.Delay(10);
        Debug.Log("Still this Method Undifined");
    }
}
