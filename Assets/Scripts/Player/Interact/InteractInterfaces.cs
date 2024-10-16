using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInteractCallBackReceivable
{
}

namespace InteractCallBacks 
{
    public interface IEquippable
    {
        void Equip(GameObject go);
    }

}




public interface IInteractable
{
    bool CanInteract();
    string GetInteractionMessage();
    void OnInteract(IInteractCallBackReceivable caller);

}


