using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// テスト用のギミック
/// </summary>
public class TestGimmick : StageGimmickBase
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

    /// <summary>
    /// テスト用
    /// </summary>
    public void ClearTestButton()
    {
        ClearActive(true);
    }

    private void ClearTest()
    {
        Debug.Log($"{gameObject.name}：クリア処理");
    }

    private void OnDisable()
    {
        _observer.OnAllGimmicksClear -= ClearTest;
    }
}
