using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

public class PlayerManager : SingletonMonoBehavior<PlayerManager>
{
    private GameObject _playerRef = null;
    private readonly string PlayerAddressableAddress = "";//Addressableのアドレスを指定
    void Start()
    {
        _playerRef = FindAnyObjectByType<PlayerStateMachine>().gameObject;
    }

    public bool DoesPlayerExist() { return  _playerRef != null; }
    
    public async UniTask InstanciatePlayer()
    {

    }
    
}
