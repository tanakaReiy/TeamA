using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using System;
using UniRx;

public class PlayerManager : SingletonMonoBehavior<PlayerManager>
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnGameStarted()
    {
        var inst = Instance;//生成
    }


    private GameObject _playerRef = null;
    private readonly string PlayerAddressableAddress = "PlayerPrefab";//Addressableのアドレスを指定
    
   

    /// <summary>
    /// Playerが存在する場合trueを返します、Playerを生成はしません
    /// </summary>
    /// <param name="playerRef"></param>
    /// <returns></returns>
    public bool TryGetPlayerRef(out GameObject playerRef)
    {
        if(DoesPlayerExist()) { playerRef = _playerRef;return true; }
        else { playerRef = null; return false; }
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        _playerRef = FindAnyObjectByType<PlayerStateMachine>().gameObject;
    }

    public bool DoesPlayerExist() { return  _playerRef != null; }

    /// <summary>
    ///Playerを指定した絶対位置に生成します。指定しない場合はPrefabの値に従います
    /// </summary>
    /// <param name="generatePos"></param>
    /// <returns>生成されたPlayerへの参照</returns>
    public async UniTask<GameObject> InstanciatePlayerAsync(Vector3? generatePos = null)
    {
        _playerRef = await Addressables.InstantiateAsync(PlayerAddressableAddress);
        if(generatePos.HasValue) { _playerRef.transform.GetChild(0).position = generatePos.Value; }
        return _playerRef;
    }

    public IObservable<GameObject> InstanciatePlayerObservable(Vector3? generatePos = null)
    {
        AsyncSubject<GameObject> playerAsyncSubject = new();
        UniTask.Create(async () =>
        {
            playerAsyncSubject.OnNext(await InstanciatePlayerAsync(generatePos));
            playerAsyncSubject.OnCompleted();
        }).Forget();
        return playerAsyncSubject;
    }

    /// <summary>
    /// Playerの削除と生成を同一フレームで行う場合isImmediateはtrueにしてください
    /// </summary>
    /// <param name="isImmediate"></param>
    public void DestroyPlayer(bool isImmediate = true)
    {
        if(_playerRef == null) return;
        
        if(isImmediate )
        {
            DestroyImmediate(_playerRef);
        }
        else
        {
            Destroy(_playerRef);
        }
    }
    
}
