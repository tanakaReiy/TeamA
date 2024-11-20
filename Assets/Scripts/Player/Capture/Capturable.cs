using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alchemy.Inspector;
using Ability;
using UnityEngine.Events;

/// <summary>
/// キャプチャされる対象につけてください
/// キャプチャした際になんのアビリティを得れるかをCapturableAbilityで指定してください
/// </summary>
public class Capturable : MonoBehaviour,IDetectable
{
    [field: SerializeField, SerializeReference]
    public WandManager.CaptureAbility CapturableAbility { get; private set; }

    public UnityEvent OnCaptured;

    public bool IsEnableDetect => true;



    private void Awake()
    {
        if(!TryGetComponent<Collider>(out var c))
        {
            Debug.LogError("Coliderが存在しないためこのCapturableは検出されません");
        }
    }


}


