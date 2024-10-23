using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageSystem;

[RequireComponent(typeof(PlayerStatus))]
public class PlayerDamageReceiver : MonoBehaviour, IDamagable
{
    private PlayerStatus _status;
    private void Start()
    {
        _status = GetComponent<PlayerStatus>();
    }

    public void ApplyDamage(float damage, IDamageArg arg = null)
    {
        _status.Health.Value -= damage;
    }
}
