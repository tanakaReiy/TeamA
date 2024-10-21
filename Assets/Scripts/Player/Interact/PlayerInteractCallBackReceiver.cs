using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InteractCallBacks;


/// <summary>
/// インタラクトした後のコールバックを受け取るクラスです
/// </summary>
[RequireComponent(typeof(StateMachine))]
[RequireComponent (typeof(SocketManager))]
[RequireComponent(typeof (PlayerStatus))]
public class PlayerInteractCallBackReceiver : MonoBehaviour, IInteractCallBackReceivable,IEquippable
{

    private const string AttachSocketName = "StaffSocket";

    private SocketManager _socketManager;
    private PlayerStateMachine _stateMachine;
    private PlayerStatus _status;


    private void Start()
    {
        _socketManager = GetComponent<SocketManager>();
        _stateMachine = GetComponent<PlayerStateMachine>();
        _status = GetComponent<PlayerStatus>();
    }
    public void Equip(GameObject go)
    {
        _socketManager.DetachFrom(AttachSocketName);
        _socketManager.AttachTo(go.GetComponent<IAttachable>(), AttachSocketName);
        _status.Ability = PlayerStatus.CapureAbility.ExampleAbility;
    }



}
