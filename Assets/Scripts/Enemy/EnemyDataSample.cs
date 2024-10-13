using Alchemy.Inspector;
using System;
using UnityEngine;

[CreateAssetMenu]
public class EnemyDataSample : ScriptableObject
{
    [LabelText("生成するエネミーのプレハブ")]
    [SerializeField] private GameObject _enemyPrefab;

    [LabelText("生成できる最大数")]
    [SerializeField] private int _maxEnemyCnt;

    [LabelText("エネミーの挙動")]
    [SerializeField, SerializeReference] private IMovePatternEnemy _movePatern;
}

public interface IMovePatternEnemy { }

[Serializable]
/// <summary>巡回するやつ</summary>
public sealed class PatrolEnemy : IMovePatternEnemy
{
    [SerializeField] private Vector3[] _patrolPositions;
    /// <summary>
    /// ゲッター
    /// </summary>
    public Vector3[] GetPatrolPositions => _patrolPositions;
    [ReadOnly] private int _lastPositionIndex;
}


[Serializable]
/// <summary>一か所を守るやつ</summary>
public sealed class GuardEnemy : IMovePatternEnemy
{
    [SerializeField] private Vector3 _guardPosition;
}


[Serializable]
/// <summary>自由に動き回るやつ</summary>
public sealed class FreeMoveEnemy : IMovePatternEnemy
{
}