using System;
using UnityEngine;

//–¢À‘•

[Serializable]
/// <summary>„‰ñ‚·‚é‚â‚Â</summary>
public sealed class PatrolEnemy : IMovePatternEnemy
{
    [SerializeField] private Transform[] _patrolPositions;

    public (Vector3 position, Vector3 direction) NextTarget()
    {
        return (Vector3.zero, Vector3.zero);
    }
}
