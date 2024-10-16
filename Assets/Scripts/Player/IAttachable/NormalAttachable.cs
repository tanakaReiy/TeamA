using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class NormalAttachable : MonoBehaviour,IAttachable
{
    public Transform GetTransform()
    {
        return transform;
    }

    public void OnAttach()
    {
        Debug.Log($"{gameObject.name}がアタッチされました");
    }

    public void OnDetach()
    {
        Debug.Log($"{gameObject.name}がデタッチされました");

    }

}
