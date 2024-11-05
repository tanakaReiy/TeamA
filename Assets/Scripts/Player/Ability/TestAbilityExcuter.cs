using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAbilityExcuter : OnceDetector<IAbilityDetectable>, IAbilityExcuter
{

    [SerializeField] WandManager.CaptureAbility ability = WandManager.CaptureAbility.Test1;



    public Transform GetTransform()
    {
        return transform;
    }

    public void OnAttach()
    {
        transform.localPosition = Vector3.zero;
    }

    public void OnDetach()
    {
        Destroy(this.gameObject);
    }

    public void PerformAbility()
    {
        StartDetection((detectable) =>
        {
            detectable.OnAbilityDetect(ability);
            EndDetection();
        });
    }

    public void CancelAbility()
    {
        EndDetection();
    }
}
