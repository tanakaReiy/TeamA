using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTest : MonoBehaviour, IGimmick
{
    Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }
    public void Activate()
    {
        _animator.SetTrigger("Open");
        Debug.Log("ドアオープン");
    }
}
