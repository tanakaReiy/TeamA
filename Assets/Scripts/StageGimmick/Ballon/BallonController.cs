using Alchemy.Inspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitMotion;
using LitMotion.Extensions;
using Cysharp.Threading.Tasks;
using System.Net;

public class BallonController : MonoBehaviour, IInteractable
{
    [LabelText("移動距離")]
    [SerializeField] private float _moveDistance = 3.0f;
    [LabelText("移動時間")]
    [SerializeField] private float _moveDuration = 3.0f;
    [LabelText("停止時間")]
    [SerializeField] private float _pauseDuration = 2.0f;

    private Vector3 _startPos;
    private Vector3 _endPos;
    private MotionHandle _moveMotion;
    private bool _isMoving;
    private bool _isPause;
    private bool _isCountingDown;


    // Start is called before the first frame update
    private void Start()
    {
        _startPos = this.transform.position;
        _endPos = new Vector3(_startPos.x, _startPos.y + _moveDistance, _startPos.z);
        // 上昇と下降のモーションを一つにまとめて作成し、Yoyoループを設定
        _moveMotion = LMotion
            .Create(_startPos, _endPos, _moveDuration)
            .WithLoops(-1, LoopType.Yoyo) // 無限ループのYoyoモーション
            .WithEase(Ease.InOutCubic)
            .BindToPosition(transform);

        _moveMotion.PlaybackSpeed = 0f; // 初期状態で停止
    }

    private void Update()
    {
        //CheckPausePosition(); //位置チェックの処理
    }

    //Motionを再生するメソッド
    private void StartMotion()
    {
        if(_isCountingDown)
        {
            SwitchPause();
        }
        else
        {
            _isMoving = true;
            _moveMotion.PlaybackSpeed = 1f;
        } 
    }

    //Motionを止めるメソッド
    private void StopMotion()
    {
        if(_isCountingDown)
        {
            SwitchPause();
        }
        else
        {
            _isMoving = false;
            _moveMotion.PlaybackSpeed = 0f;
        }
    }

    //カウントダウン処理
    private async UniTask StartCountdown()
    {
        _isCountingDown = true;
        float remainingTime = _pauseDuration;
        while (remainingTime > 0)
        {

            if (_isPause)
            {
                // 一時停止している場合は待機
                await UniTask.Yield();
                continue;
            }

            // 1秒ごとに残り時間を減らす
            await UniTask.Delay(1000);
            remainingTime--;

            if (remainingTime <= 0)
            {
                Debug.Log("カウントダウン終了");
                _isPause = false;
                _isCountingDown = false;
                if(_isMoving)
                {
                    StartMotion();
                }
                break;
            }
        }
    }

    // カウントダウンの一時停止を切り替える
    public void SwitchPause()
    {
        _isPause = !_isPause;
        Debug.Log(_isPause ? "一時停止中" : "再開");
    }

    //始点か終点に着いたか判定するメソッド
    private void CheckPausePosition()
    {
        // 現在の位置が始点または終点に到達したかどうかを確認
        if (Vector3.Distance(transform.position, _startPos) < 0.01f)
        {
            if (!_isCountingDown) // カウントダウンが開始されていない場合
            {
                Debug.Log("始点に到達");
                StartCountdown().Forget(); // 始点に到達したらカウントダウンを開始
            }
        }
        else if (Vector3.Distance(transform.position, _endPos) < 0.01f)
        {
            if (!_isCountingDown) // カウントダウンが開始されていない場合
            {
                Debug.Log("終点に到達");
                StartCountdown().Forget(); // 終点に到達したらカウントダウンを開始
            }
        }
    }

    [Button]
    public void TestButton()
    {
        if (_isMoving)
            StopMotion();
        else
            StartMotion();
    }

    public bool CanInteract()
    {
        return true;
    }

    public string GetInteractionMessage()
    {
        return "動かす";
    }

    public void OnInteract(IInteractCallBackReceivable caller)
    {
        if (_isMoving)
            StopMotion();
        else
            StartMotion();
    }

    //移動床のコライダーに触れた時の処理
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 触れたobjの親を移動床にする
            collision.transform.SetParent(transform);
            Debug.Log("当たった");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 触れたobjの親をなくす
            collision.transform.SetParent(null);
            Debug.Log("離れた");
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying) return;

        // ギズモで移動範囲を表示
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y + _moveDistance, startPosition.z);

        Gizmos.color = Color.red;
        Gizmos.DrawCube(endPosition, this.transform.localScale);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(startPosition, endPosition);
    }
}
