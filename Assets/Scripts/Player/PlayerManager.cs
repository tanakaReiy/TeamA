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
        var inst = Instance;//����
    }


    private GameObject _playerRef = null;
    private readonly string PlayerAddressableAddress = "PlayerPrefab";//Addressable�̃A�h���X���w��
    
   

    /// <summary>
    /// Player�����݂���ꍇtrue��Ԃ��܂��APlayer�𐶐��͂��܂���
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
    ///Player���w�肵����Έʒu�ɐ������܂��B�w�肵�Ȃ��ꍇ��Prefab�̒l�ɏ]���܂�
    /// </summary>
    /// <param name="generatePos"></param>
    /// <returns>�������ꂽPlayer�ւ̎Q��</returns>
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
    /// Player�̍폜�Ɛ����𓯈�t���[���ōs���ꍇisImmediate��true�ɂ��Ă�������
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
