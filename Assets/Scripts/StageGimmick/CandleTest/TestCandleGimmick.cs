using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCandleGimmick : StageGimmickBase
{
    private StageGimmickObserver _observer;

    private void Awake()
    {
        _observer = FindObjectOfType<StageGimmickObserver>();
        _observer.OnAllGimmicksClear += ClearTest;
    }
    protected override void ClearActive()
    {
        base.ClearActive();
    }
    private void ClearTest()
    {
        //ここにスイッチを出現させる処理
        Debug.Log("スイッチ出現!");
    }
}
