using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Alchemy.Serialization;
using Alchemy.Inspector;
using UniRx.Triggers;
using UniRx;

[AlchemySerialize]
public partial class SocketManager : MonoBehaviour
{
    [AlchemySerializeField,NonSerialized]
    private Dictionary<string, Socket> _soketsTable = new();


    private Dictionary<IAttachable, Socket> _attachedObjects = new();
    private void Awake()
    {
        CreateSocketTable();     
    }

    public void AttachTo(IAttachable target,string SocketName)
    {
        var socket = GetSocket(SocketName);
        target.GetTransform().SetParent(socket.transform);
        target.GetTransform().localPosition = Vector3.zero;
        target.GetTransform().localRotation = Quaternion.identity;
        target.OnAttach();
        _attachedObjects.Add(target, socket);
        target.GetTransform().OnDestroyAsObservable()
            .Subscribe(_ =>
            {
                _attachedObjects.Remove(target);
            }).AddTo(target.GetTransform());
        
    }
    public void DetachFrom(string SocketName)
    {
        foreach(var pair in _attachedObjects)
        {
            if (pair.Value.SocketName.Equals(SocketName))
            {
                pair.Key.GetTransform().SetParent(null);
                _attachedObjects.Remove(pair.Key);
                pair.Key.OnDetach();
                break;
            }
        }
    }

    public void DetachThis(IAttachable target)
    {
        target.GetTransform().SetParent(null);
        target.OnDetach();
        _attachedObjects.Remove(target);
    }

    [Button]
    public void CreateSocketTable()
    {
        
        _soketsTable.Clear();
        Socket[] sockets = GetComponentsInChildren<Socket>();
        for (int i = 0; i < sockets.Length; i++)
        {
            _soketsTable.Add(sockets[i].SocketName, sockets[i]);
        }
    }

    public Socket GetSocket(string socketName)
    {
        if (_soketsTable.ContainsKey(socketName))
        {
            return _soketsTable[socketName];
        }
        else
        {
            throw new System.Exception($"SocketTable‚É{socketName}‚ª‘¶Ý‚µ‚Ü‚¹‚ñ");
        }
    }

    
}

public interface IAttachable
{
    Transform GetTransform();
    void OnAttach();
    void OnDetach();
}