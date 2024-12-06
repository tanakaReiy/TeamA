using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleGimmickChacker : StageGimmickBase
{
    private StageGimmickObserver _observer;
    private void Awake()
    {
        _observer = GameObject.FindObjectOfType<StageGimmickObserver>();
        _observer.OnAllGimmicksClear += ClearTest;
    }
    protected override void ClearActive(bool changed)
    {
        base.ClearActive(changed);
    }

    private void ClearTest()
    {
        Debug.Log($"{gameObject.name}：クリア処理");
    }

    private void OnDisable()
    {
        _observer.OnAllGimmicksClear -= ClearTest;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Donuts"))
        {
            ClearActive(true);
        }
    }
}
