using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICapturableEnemy
{
    public virtual void CaptureStatusSet(ref PlayerStatus playerStatus)
    {

    }
}
