using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTest : MonoBehaviour
{
    Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

   public void OpenDoor()
    {
        _animator.SetTrigger("Open");
        Debug.Log("ドアオープン");
    }
}
