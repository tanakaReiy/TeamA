using Alchemy.Inspector;
using UnityEngine;

public class Surtr : EnemyBase
{
    [Title("å≈óLê›íË")]
    [SerializeField] float _sampleFloat = 1.0f;
    //protected sealed override async UniTask OnAttackedActionAsync(CancellationToken cancellationToken)
    //{

    //}
    //protected sealed override async UniTask OnDamagedActionAsync(CancellationToken cancellationToken)
    //{

    //}
    //protected sealed override async UniTask OnDeathActionAsync(CancellationToken cancellationToken)
    //{

    //}
#if UNITY_EDITOR
    [Title("åüèÿóp")]

    [Button]
    private void Damage(int damage)
    {
        if (base.ApplyDamage(damage))
        {
            Debug.Log($"Damaged enemy:{this.name}");
        }
        else
        {
            Debug.Log($"Fail Damage enemy:{this.name}");
        }
    }
#endif
}
