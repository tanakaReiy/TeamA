using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// ステージギミックのベースクラス
/// </summary>
public abstract class StageGimmickBase : MonoBehaviour
{
    //ギミックのクリア状態を管理するプロパティ,trueにすることによりクリア通知を送る
    protected bool isClear = false;

    /// <summary>
    /// クリアするまで待つ処理
    /// </summary>
    public async UniTask WaitClearFlag(CancellationToken cancellationToken)
    {
        await UniTask.WaitUntil(() => isClear, cancellationToken: cancellationToken);
        Debug.Log($"{this.gameObject.name}がクリアされました");
    }

    /// <summary>
    /// <br>クリア時に呼ぶ処理</br>
    /// <br>※オーバーライド時にはisClearをtrueにする処理を忘れずに</br>
    /// </summary>
    protected virtual void ClearActive(bool changeIsClear)
    {
        isClear = changeIsClear;
    }
}
