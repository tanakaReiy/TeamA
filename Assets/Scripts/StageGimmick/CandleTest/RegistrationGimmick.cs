using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
interface IGimmick
{
    void Activate();
}

interface IActivatable
{
    event Action OnActivated;
}

[System.Serializable]
public class GimmickPair
{
    public MonoBehaviour _activatable;
    public MonoBehaviour _gimmick; //インスペクターから操作できるように一旦MonoBehaviourにしてる
}

public class RegistrationGimmick : MonoBehaviour
{
    [SerializeField] private List<GimmickPair> _gimmickPairs;
    [SerializeField] private GameObject _doorPendant;
    StageGimmickObserver _observer;
    private void Start()
    {
        _observer = FindAnyObjectByType<StageGimmickObserver>();
        foreach (var pair in _gimmickPairs)
        {
            if (pair?._activatable is IActivatable activatable && pair?._gimmick is IGimmick gimmick)
            {
                activatable.OnActivated += gimmick.Activate;
            }
            else
            {
                Debug.Log("必要なインターフェースが実装されてません");
            }
        }
        _doorPendant?.SetActive(false);
        _observer.OnAllGimmicksClear += AllGimmickClear;
    }
    public void AllGimmickClear()
    {
        _doorPendant.SetActive(true);
        CRIAudioManager.SE.Play3D(Vector3.zero, "CueSheet_0", "SE_pendant_deru");
    }
    private void OnDisable()
    {
        _observer.OnAllGimmicksClear -= AllGimmickClear;
    }
}


