using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAbilityDetectable : MonoBehaviour,IAbilityDetectable
{
    public bool IsEnableDetect => true;

    public void OnAbilityDetect(WandManager.CaptureAbility ability)
    {
        switch (ability)
        {
            case WandManager.CaptureAbility.None:
                break;
            case WandManager.CaptureAbility.Test1:
                Debug.Log("Test1 Ability Detected");
                break;
            case WandManager.CaptureAbility.Test2:
                Debug.Log("Test2 Ability Detected");

                break;
            case WandManager.CaptureAbility.Test3:
                break;
        }
    }
}
