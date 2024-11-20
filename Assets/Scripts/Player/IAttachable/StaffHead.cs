using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffHead : MonoBehaviour, IAttachable
{
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
}
