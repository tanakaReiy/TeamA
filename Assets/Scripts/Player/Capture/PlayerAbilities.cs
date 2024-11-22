using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Ability;

[Serializable]
public class Surtr_Ability : IPlayerAbility
{
    public void PerformAbility()
    {
        Debug.Log("SultrAbility");
    }
}

