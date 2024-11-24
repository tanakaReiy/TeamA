using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

//未実装

[Serializable]
/// <summary>巡回するやつ</summary>
public sealed class PatrolEnemy : IMovePatternEnemy
{
    [Serializable]
    public class PatrolPointData
    {
        public Vector3 Position;
        public float Direction;
    }


    [SerializeField] PatrolPointData[] _patrolPositions;

    private int _nowTargetIndex = -1;

    public void OnDisposed()
    {
        _nowTargetIndex = -1;
    }

    (Vector3 position, float direction)[] IMovePatternEnemy.GetAllTargets()
    {
        (Vector3 position, float direction)[] positions = new (Vector3 position, float direction)[_patrolPositions.Length];
        for (int i= 0; i < _patrolPositions.Length; i++)
        {
            positions[i] = (_patrolPositions[i].Position, _patrolPositions[i].Direction);
        }
        return positions;
    }
    public (Vector3 position, float direction) GetNextTarget()
    {
        //もしパトロール地点が未定義ならダミーを返す
        if(_patrolPositions.Length == 0)
        {
            return (Vector3.zero, 0);
        }
        //内部のターゲットインデックスを増やす
        _nowTargetIndex++;
        if(_nowTargetIndex >= _patrolPositions.Length)
        {
            _nowTargetIndex = 0;
        }//インデックスを元にデータを返す
        return (_patrolPositions[_nowTargetIndex].Position, _patrolPositions[_nowTargetIndex].Direction);
    }

    public async UniTask NextTargetActionAsync(Quaternion rotation, Transform transform, CancellationToken token)
    {
        await UniTask.Delay(10);
        Debug.Log("Still this Method Undifined");
    }
}
