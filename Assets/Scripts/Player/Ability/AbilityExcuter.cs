using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �A�r���e�B�����s����@�\������Interface
/// </summary>
public interface IAbilityExcuter : IAttachable
{
    void PerformAbility();
    void CancelAbility();
}
