using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInteractCallBackReceivable
{
}

namespace InteractCallBacks 
{


}




public interface IInteractable
{
    abstract void OnInteract(IInteractCallBackReceivable caller);

    InRangeData OnEnterInteractRange(IInteractCallBackReceivable caller)
    {
        return new InRangeData { _inRangeText = ("インタラクトする") };
    }
}


