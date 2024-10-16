using System;
using UnityEngine;

[Serializable]
/// <summary>„‰ñ‚·‚é‚â‚Â</summary>
public sealed class PatrolEnemy : IMovePatternEnemy
{
    [SerializeField] private Transform[] _patrolPositions;

    private Action MoveEnemyPatrol;
}
