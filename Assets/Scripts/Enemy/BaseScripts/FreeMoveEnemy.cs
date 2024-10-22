using System;
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
}