using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageSystem
{
    public interface IDamagable
    {
        void ApplyDamage(float damage,IDamageArg arg = null);
    }

    public interface IDamageArg { }

}


