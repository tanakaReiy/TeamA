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
        CRIAudioManager.SE.Play3D(this.transform.position,"CueSheet_0", "SE_door_open");
        _doorAnimator?.SetTrigger("Open");
    }
}
