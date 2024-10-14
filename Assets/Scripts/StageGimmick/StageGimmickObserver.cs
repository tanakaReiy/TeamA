using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditorInternal;
using UnityEngine;

/// <summary>
/// すべてのギミックをクリアしたときに通知をするクラス
/// </summary>
public class StageGimmickObserver : MonoBehaviour
{
    [Header("現在の階層のギミック")]
    [SerializeField] private StageGimmickBase[] _stageGimmicks;

    // キャンセル用のトークンソース
    private CancellationTokenSource _cancellationTokenSource;

    // クリア時のイベントデリゲート
    public event Action OnAllGimmicksClear;

    private void Start()
    {
        if(_stageGimmicks != null)
        {
            //キャンセレーショントークンソースの初期化
            _cancellationTokenSource = new CancellationTokenSource();
            //ギミッククリアを監視する処理を開始
            MonitorGimmicks(_cancellationTokenSource.Token);
        }
        else
        {
            Debug.LogWarning("ギミックが登録されていません");
        }
    }

    /// <summary>
    /// 全てのギミックがクリアされるまで待つ
    /// </summary>
    private async void MonitorGimmicks(CancellationToken cancellationToken)
    {
        var gimmickTasks = new UniTask[_stageGimmicks.Length];

        //それぞれのギミックのクリアを待つ処理を呼び出す
        for(int i = 0; i < _stageGimmicks.Length; i++)
        {
            gimmickTasks[i] = _stageGimmicks[i].WaitClearFlag(cancellationToken);
        }
        
        try
        {
            //すべてのギミックがクリアされるまで待つ
            await UniTask.WhenAll(gimmickTasks);
            //ギミックが完了した通知を飛ばす
            NotifyGimmicksClear();
        }
        catch(OperationCanceledException)
        {
            Debug.Log("クリア処理がキャンセルされました");
        }  
    }

    /// <summary>
    /// ギミッククリア通知を発行する
    /// </summary>
    private void NotifyGimmicksClear()
    {
        Debug.Log("すべてのギミックがクリアされました！");
        OnAllGimmicksClear?.Invoke(); // イベントを発行
    }

    private void OnDisable()
    {
        //キャンセル処理を実行する
        _cancellationTokenSource?.Cancel();
        //破棄する
        _cancellationTokenSource?.Dispose();
    }
}
