using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandHeader : MonoBehaviour,IAttachable
{
    [field: SerializeField] public WandManager.CaptureAbility Ability { get; private set; }
    [field: SerializeField] public AnimationClip OverrideAnimationClip { get; private set; }
    [field: SerializeField] public bool OverrideRangeSettings { get; private set; }
    [field:SerializeField] public Vector3 Offset { get; private set; }
    [field: SerializeField] public float Radius { get; private set; }
    [field: SerializeField] public LayerMask Mask { get; private set; }

    public Transform GetTransform()
    {
        return transform;
    }

    public void OnAttach()
    {
        
    }

    public void OnDetach()
    {
        Destroy(gameObject);
    }
}
