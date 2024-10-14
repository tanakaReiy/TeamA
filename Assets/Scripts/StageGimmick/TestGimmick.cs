using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// テスト用のギミック
/// </summary>
public class TestGimmick : StageGimmickBase
{
    protected override void ClearActive()
    {
        base.ClearActive();
    }

    /// <summary>
    /// テスト用
    /// </summary>
    public void ClearTestButton()
    {
        ClearActive();
    }
}
