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
    protected override void ClearActive(bool changeIsClear)
    {
        base.ClearActive(changeIsClear);
    }
    private void ClearTest()
    {

    }
}
