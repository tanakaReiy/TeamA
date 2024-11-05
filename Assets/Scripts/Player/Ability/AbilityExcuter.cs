using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アビリティを実行する機能も持つInterface
/// </summary>
public interface IAbilityExcuter : IAttachable
{
    void PerformAbility();
    void CancelAbility();
}
