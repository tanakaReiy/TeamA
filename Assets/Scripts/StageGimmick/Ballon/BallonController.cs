using Alchemy.Inspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitMotion;
using LitMotion.Extensions;
using Cysharp.Threading.Tasks;
using System.Threading;

public class BallonController : MonoBehaviour, IInteractable
{
    [LabelText("移動距離")]
    [SerializeField] private float _moveDistance = 3.0f;
    [LabelText("移動時間")]
    [SerializeField] private float _moveDuration = 3.0f;
    [LabelText("停止時間")]
    [SerializeField] private float _pauseDuration = 2.0f;

    [Header("当たり判定")]
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _size;

    private Vector3 _startPos;
    private Vector3 _endPos;
    private MotionBuilder<Vector3, NoOptions, LitMotion.Adapters.Vector3MotionAdapter> _upMoveBuilder;
    private MotionBuilder<Vector3, NoOptions, LitMotion.Adapters.Vector3MotionAdapter> _downMoveBuilder;
    private MotionHandle _upMoveMotion;
    private MotionHandle _downMoveMotion;
    private bool _isPause = true;
    private bool _isCountingDown;
    private bool _isUp = true;

    private CharacterMovement _cache = null;


    // Start is called before the first frame update
    private void Start()
    {
        _startPos = this.transform.position;
        _endPos = new Vector3(_startPos.x, _startPos.y + _moveDistance, _startPos.z);

        _downMoveBuilder = LMotion
            .Create(_endPos, _startPos, _moveDuration)
            .WithEase(Ease.InOutCubic)
            .WithOnComplete(async () =>
            {
                CancellationTokenSource token = new CancellationTokenSource();
                await StartCountdown(token.Token);
            })
            .Preserve();
        _downMoveMotion = _downMoveBuilder.BindToPosition(transform);
        
        // 上昇と下降をまとめる
        _upMoveBuilder = LMotion
            .Create(_startPos, _endPos, _moveDuration)
            .WithEase(Ease.InOutCubic)
            .WithOnComplete(async () =>
            {
                CancellationTokenSource token = new CancellationTokenSource();
                await StartCountdown(token.Token);
            })
            .Preserve();
        _upMoveMotion = _upMoveBuilder.BindToPosition(transform);

        // 初期状態で停止
        _upMoveMotion.PlaybackSpeed = 0f;
        _downMoveMotion.PlaybackSpeed = 0f;
    }

    private void Update()
    {
        CatchPlayer();
    }

    //Motionを再生するメソッド
    private void StartMotion()
    {
        SwitchPause();
        if(!_isCountingDown)
        {
            PlayCurrentMotion();
        }
    }

    //Motionを止めるメソッド
    private void StopMotion()
    {
        SwitchPause();
        if (!_isCountingDown)
        {
            if (_isUp)
            {
                _upMoveMotion.PlaybackSpeed = 0f;
            }
            else
            {
                _downMoveMotion.PlaybackSpeed = 0f;
            }
        }
    }

    //LMotionの呼出しを行う
    private void PlayCurrentMotion()
    {
        if(_isUp)
        {
            _upMoveMotion.ToDisposable().Dispose();
            _upMoveMotion = _upMoveBuilder.BindToPosition(transform);
        }
        else
        {
            _downMoveMotion.ToDisposable().Dispose();
            _downMoveMotion = _downMoveBuilder.BindToPosition(transform);
        }
    }

    // カウントダウンの一時停止を切り替える
    public void SwitchPause()
    {
        _isPause = !_isPause;
    }

    //カウントダウンメソッド
    private async UniTask StartCountdown(CancellationToken cancellationToken)
    {
        if (_isCountingDown) return;

        _isCountingDown = true;
        float time = 0;
        //停止時間
        while(_pauseDuration >= time)
        {
            if(!_isPause)
            {
                time += Time.deltaTime;
            }
            await UniTask.NextFrame(cancellationToken);
        }
        _isCountingDown = false;

        await UniTask.WaitUntil(() => !_isPause);

        SwitchDirection();
        PlayCurrentMotion();
    }

    private void SwitchDirection()
    {
        _isUp = !_isUp;
    }

    [Button]
    public void TestButton()
    {
        if (_isPause)
            StartMotion();
        else
            StopMotion();
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
        if (_isPause)
            StopMotion();
        else
            StartMotion();
    }

    private void CatchPlayer()
    {
        var colliders = Physics.OverlapBox(transform.position + transform.TransformVector(_offset), _size / 2f);
        bool found = false;
        foreach (var collider in colliders)
        {
            if (collider.gameObject.TryGetComponent(out CharacterMovement characterMovement))
            {
                found = true;
                if (characterMovement == _cache) { return; }
                characterMovement.SetFollowTarget(transform);
                _cache = characterMovement;
            }
        }
        if (!found && _cache != null)
        {
            _cache.SetFollowTarget(null);
            _cache = null;
        }
    }

    private void OnDestroy()
    {
        _upMoveBuilder.Dispose();
        _downMoveBuilder.Dispose();
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

        //当たり判定
        Gizmos.DrawCube(transform.position + transform.TransformVector(_offset), _size);
    }
}
