using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InteractCallBacks;

public class PlayerInteractCallBackReceiver : MonoBehaviour, IInteractCallBackReceivable,IEquippable
{
    [SerializeField] PlayerStateMachine _stateMachine;

    private const string AttachSocketName = "StaffSocket";

    private SocketManager _socketManager;




    private void Start()
    {
        _socketManager = GetComponent<SocketManager>();
    }
    public void Equip(GameObject go)
    {
        _socketManager.DetachFrom(AttachSocketName);
        _socketManager.AttachTo(go.GetComponent<IAttachable>(), AttachSocketName);
    }



}
