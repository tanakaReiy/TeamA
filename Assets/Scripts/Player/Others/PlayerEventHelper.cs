using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public static class PlayerEventHelper
{
    public static Subject<WandManager.CaptureAbility> OnCaptureAbility = new();
} 
