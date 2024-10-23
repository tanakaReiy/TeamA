using Ability;
using UnityEngine;

public class SurtrCaptrable : IPlayerAbility
{
    public void PerformAbility()
    {
        Debug.Log("Captured Surtr");
    }
}