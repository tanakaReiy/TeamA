using System;
using UnityEngine;

[Serializable]
/// <summary>一か所を守るやつ</summary>
public sealed class GuardEnemy : IMovePatternEnemy
{
    [SerializeField] private Transform _guardPosition;
    

}
