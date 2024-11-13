using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTest : MonoBehaviour, IGimmick
{
    Animator _doorAnimator;
    void Start()
    {
        _doorAnimator = GetComponent<Animator>();
    }
    public void Activate()
    {
        _doorAnimator?.SetTrigger("Open");
        Debug.Log("ドアオープン");
    }
}
