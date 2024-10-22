using Alchemy.Inspector;
using System;
using UnityEngine;

[Serializable]
/// <summary>一か所を守るやつ</summary>
public sealed class GuardEnemy : IMovePatternEnemy
{
    [SerializeField] private Vector3 _guardPosition;
    [SerializeField] private Vector3 _guardFrontAngle;
    public (Vector3 position, Vector3 direction) NextTarget()
    {
        return (_guardPosition, _guardFrontAngle);
    }
#if UNITY_EDITOR
    /// <summary>
    /// 
    /// ※この仮想クラス以外で呼び出さないこと
    /// </summary>
    [Button]
    public void GenerateGuradPosition() 
    {

    }
#endif
}
