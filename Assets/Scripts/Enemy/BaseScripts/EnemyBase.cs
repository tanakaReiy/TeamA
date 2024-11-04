using Ability;
using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// <br>エネミーのステートパターンをごり押しで実装している</br>
/// <br>エネミーの個別スクリプトはこれを継承することを前提に作成</br>
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBase : MonoBehaviour, IInteractable
{
    private NavMeshAgent _navMeshAgent;
    private NavMeshHit _navMeshHit;

    /// <summary>
    /// 次の目的地をセットするFunc
    /// EnemySpawnerから個別に設定
    /// </summary>
    public Func<(Vector3 position, Vector3 rotation)> GetNextPosition;

    public Func<Quaternion, Transform, CancellationToken, UniTask> GetNextGoalAction;

    //Playerの位置を知るための参照
    //後にシーンにあるエネミースポナーからPlayerのtransformを参照渡しする方法に変更
    //EnemyBaseから変更を加えない
    [ReadOnly]
    public Transform _playerTransform;

    [ReadOnly]
    [SerializeField] private bool _initialized = false;

    [Title("基本設定")]

    [LabelText("設定するHP")]
    [SerializeField] private int _enemyHp = 1;

    [LabelText("設定する上限の移動速度")]
    [SerializeField] private float _enemyMaxSpeed = 6;

    [LabelText("設定する回転速度（deg/s）")]
    [SerializeField] private float _enemyAngularSpeed = 240;

    [LabelText("地面からの高さ")]
    [SerializeField] private float _enemyBaseOffset = 0.5f;

    [LabelText("現在の状態")]
    [SerializeField] private EnemyState _enemyState = EnemyState.Move;

    [LabelText("プレイヤーを発見できる距離")]
    [SerializeField] private float _searchablePlayerDistance = 5;

    [LabelText("プレイヤーを視野できる角度(正面からの角度)")]
    [SerializeField] private float _fieldOfViewHalf = 90;

    [LabelText("攻撃可能距離")]
    [SerializeField] private float _attackableDistance = 3;

    private Vector3 _lastTarget = Vector3.zero;

    private float _searchableTargetRange = 5;

    private bool _isTargetPlayer = false;
    private bool _isSearchPlayer = false;

    private CancellationTokenSource _ctsChangeStateAction;
    private CancellationToken _cancellTokenChangeState;

    private CancellationTokenSource _ctsMovedAction;
    private CancellationToken _cancellTokenMovedAction;

    private bool _isPlayMovedAction = false;
    private bool _isPlayChangeStateAction = false;

    public void Initialize()
    {
        _ctsChangeStateAction = new CancellationTokenSource();
        _cancellTokenChangeState = _ctsChangeStateAction.Token;
        _ctsMovedAction = new CancellationTokenSource();
        _cancellTokenMovedAction = _ctsMovedAction.Token; ;

        _navMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();

        //memo
        //ここのプレイヤーのポジション参照の取得は後にシングルトンかスポナーから与えられる形に変わる
        _playerTransform = FindAnyObjectByType<PlayerInEnemyTest>().GetComponent<Transform>();

        //初期化できたかどうかのnullチェック
        if (_playerTransform && _navMeshAgent && GetNextPosition != null && GetNextGoalAction != null && OnStart())
        {
            _initialized = true;
        }
        //Navmeshレイヤーの名前は後で変える
        _navMeshAgent.agentTypeID = NavMesh.GetAreaFromName("EnemyMovable");

        _navMeshAgent.speed = _enemyMaxSpeed;
        _navMeshAgent.angularSpeed = _enemyAngularSpeed;
        _navMeshAgent.baseOffset = _enemyBaseOffset;
        NavMesh.SamplePosition(GetNextPosition().position, out _navMeshHit, _searchableTargetRange, 1);
        _navMeshAgent.destination = _navMeshHit.position;
    }
    /// <summary>
    /// <br>継承先で、本来Start()でしたい処理をここに書く/br>
    /// <br>実行はEnemyBase内でのInitialize()のnullチェック中に呼ばれる</br>
    /// <br>この中の初期化が正常に行われない場合も全体の動きが停止するように実装予定</br>
    /// </summary>
    /// <returns> 初期化できたならTrue、できてないならFalseを返す</returns>
    public virtual bool OnStart()
    {
        return true;
    }

    private void FixedUpdate()
    {
        if (!_initialized || !_navMeshAgent.isOnNavMesh)
        {
            return;
        }

        if (_enemyState == EnemyState.Move)
        {
            ActionDecisionStateMove();
        }
    }
    /// <summary>
    /// EnemyStateがMoveの時の行動を決める
    /// </summary>
    private async void ActionDecisionStateMove()
    {
        _isSearchPlayer = SearchPlayer();
        if (_isSearchPlayer)
        {
            //GoalActionAsyncをしている途中ならキャンセルをする
            if (_isPlayMovedAction)
            {
                // すでにキャンセルされているなら例外を投げる
                _cancellTokenMovedAction.ThrowIfCancellationRequested();

                //処理停止
                _ctsMovedAction.Cancel();
            }
            else
            {
                ChasePlayer();
            }
        }
        else if (_isTargetPlayer && !_isSearchPlayer)
        {
            _isTargetPlayer = false;
            //token再生成
            _ctsMovedAction = new CancellationTokenSource();
            _cancellTokenMovedAction = _ctsMovedAction.Token;
            _isPlayMovedAction = true;

            await LookAround(_cancellTokenMovedAction);

            _isPlayMovedAction = false;
            SetLastTarget();
        }
        //どうやらNavMeshAgent.remainDistanceは挙動が怪しく使い物にならない。よって直接目的地との直線距離で判定している
        else if (Vector3.Distance(_navMeshAgent.destination, this.transform.position) <= _navMeshAgent.stoppingDistance && !_isPlayMovedAction)
        {
            _lastTarget = _navMeshAgent.destination;
            (Vector3 position, Vector3 rotation) NextTargetData = GetNextPosition();
            SetNextTarget(NextTargetData.Item1);

            //token再生成
            _ctsMovedAction = new CancellationTokenSource();
            _cancellTokenMovedAction = _ctsMovedAction.Token;

            _isPlayMovedAction = true;

            await GetNextGoalAction(Quaternion.Euler(NextTargetData.rotation), this.transform, _cancellTokenMovedAction);

            _isPlayMovedAction = false;

            //nullじゃなかったら明示的に解放
            if (_ctsMovedAction != null)
            {
                _ctsMovedAction.Dispose();
                Debug.Log("Dispose ctsGoalAction");
            }
        }
    }

    /// <summary>
    /// プレイヤーが視野内に存在するかを判定
    /// </summary>
    /// <returns>視野内にいるならTrue、いないならFalse</returns>
    private bool SearchPlayer()
    {
        Vector3 toPlayerDirection = _playerTransform.position - this.transform.position;
        bool a = toPlayerDirection.magnitude < _searchablePlayerDistance;
        bool b = Vector3.Angle(transform.forward, toPlayerDirection) < _fieldOfViewHalf;
        bool c = !Physics.Raycast(transform.position, toPlayerDirection, toPlayerDirection.magnitude, -1 - (1 << LayerMask.NameToLayer("Player")));
        int L = LayerMask.NameToLayer("Player");
        bool d = Physics.Raycast(transform.position, toPlayerDirection, _searchablePlayerDistance, (int)Mathf.Pow(2, 7));

        if (a && b && c && d)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// プレイヤーが視野内のいる間、移動目標をプレイヤーの座標にする
    /// </summary>
    private void ChasePlayer()
    {
        if(!_isTargetPlayer)
        {
            _lastTarget = _navMeshAgent.destination;
        }
        if (Vector3.Distance(_playerTransform.position, this.transform.position) < _attackableDistance)
        {
            ChangeEnemyStateAsync(EnemyState.Attack);
        }
        else
        {
            _isTargetPlayer = true;
            NavMesh.SamplePosition(_playerTransform.position, out _navMeshHit, _searchableTargetRange, 1);
            _navMeshAgent.destination = _navMeshHit.position;
        }
    }

    /// <summary>
    /// プレイヤーが視野から外れたときに、移動目標を最後に設定した目標に戻す処理
    /// </summary>
    private void SetLastTarget()
    {
        NavMesh.SamplePosition(_lastTarget, out _navMeshHit, _searchableTargetRange, 1);
        _navMeshAgent.destination = _navMeshHit.position;
    }

    /// <summary>
    /// 次の目標地点を設定する
    /// </summary>
    private void SetNextTarget(Vector3 nextPosition)
    {
        if (_navMeshAgent.destination != null && _lastTarget == _navMeshAgent.destination)
        {
            _lastTarget = _navMeshAgent.destination;
            if (nextPosition != null)
            {
                NavMesh.SamplePosition(nextPosition, out _navMeshHit, _searchableTargetRange, 1);
                _navMeshAgent.destination = _navMeshHit.position;
            }
        }
    }

    private async UniTask LookAround(CancellationToken token)
    {
        Vector3 forward = this.transform.forward;
        try
        {
            await LMotion.Create(forward, Quaternion.AngleAxis(90, Vector3.up) * forward, 1)
                .WithEase(Ease.OutQuad).WithLoops(2, LoopType.Yoyo).BindToLocalEulerAngles(transform).ToUniTask(token);
            await LMotion.Create(this.transform.forward, Quaternion.AngleAxis(-90, Vector3.up) * forward, 2)
                .WithEase(Ease.OutCubic).BindToLocalEulerAngles(transform).ToUniTask(token);
        }
        catch (OperationCanceledException e)
        {
            Debug.Log("Cancel LookAround");
        }
    }

    /// <summary>
    /// エネミーの状態を変更し、敵の状態に伴ったメソッドを1回呼び出す
    /// </summary>
    /// <param name="changedEnemyState"></param>
    async protected void ChangeEnemyStateAsync(EnemyState changedEnemyState)
    {
        if (_isPlayMovedAction)
        {
            // すでにキャンセルされているなら例外を投げる
            _cancellTokenMovedAction.ThrowIfCancellationRequested();

            //処理停止
            _ctsMovedAction.Cancel();
        }
        if (_isPlayChangeStateAction)
        {
            // すでにキャンセルされているなら例外を投げる
            _cancellTokenChangeState.ThrowIfCancellationRequested();
            //処理停止
            _ctsChangeStateAction.Cancel();
            //nullじゃなかったら明示的に解放
            if (_ctsChangeStateAction != null)
            {
                _ctsChangeStateAction.Dispose();
            }
        }

        //token再生成
        _ctsChangeStateAction = new CancellationTokenSource();
        _cancellTokenChangeState = _ctsChangeStateAction.Token;

        _enemyState = changedEnemyState;

        //確認用　後で消す
        Debug.Log($"Enemy:{this.gameObject.name} change state {_enemyState}");

        switch (_enemyState)
        {
            case EnemyState.Attack:
                _navMeshAgent.enabled = false;
                _isPlayChangeStateAction = true;
                await OnAttackedActionAsync(_cancellTokenChangeState);
                _isPlayChangeStateAction = false;
                _navMeshAgent.enabled = true;

                ChangeEnemyStateAsync(EnemyState.Move);
                break;

            case EnemyState.Damage:
                _navMeshAgent.enabled = false;
                _isPlayChangeStateAction = true;
                await OnDamagedActionAsync(_cancellTokenChangeState);
                _isPlayChangeStateAction = false;
                _navMeshAgent.enabled = true;

                ChangeEnemyStateAsync(EnemyState.Move);
                break;

            case EnemyState.Death:
                GetComponent<Collider>().enabled = false;
                _navMeshAgent.enabled = false;
                _isPlayChangeStateAction = true;
                await OnDeathActionAsync(_cancellTokenChangeState);
                _isPlayChangeStateAction = false;
                _navMeshAgent.enabled = true;
                break;
        }
    }

    /// <summary>
    /// ダメージを受けた際に㏋を変更する機能　後に変更ある可能性あり
    /// </summary>
    /// <param name="damage"></param>
    public virtual void Damaged(int damage)
    {
        if (_enemyState == EnemyState.Damage || _enemyState == EnemyState.Death)
        {
            //確認用　後で消す
            Debug.Log($"Now, enemy:{this.gameObject.name} cant damaged");
            return;
        }
        _enemyHp -= damage;
        if (_enemyHp < 0)
        {
            _enemyHp = 0;
            ChangeEnemyStateAsync(EnemyState.Death);
        }
        else
        {
            ChangeEnemyStateAsync(EnemyState.Damage);
        }
    }

    /// <summary>
    /// enemyStateがAttackに切り替わった際に呼ばれる
    /// overrideする前提の関数
    /// </summary>
    virtual async protected UniTask OnAttackedActionAsync(CancellationToken token)
    {
        //確認用　後で消す
        Debug.Log($"Enemy:{this.gameObject.name} attacked！\nStart attacked action");
        
        await LMotion.Create(this.transform.position, this.transform.position + this.transform.forward * 5, 1)
            .WithEase(Ease.OutElastic).BindToPosition(this.transform).ToUniTask(token);
        Debug.Log($"Enemy:{this.gameObject.name} finish attacked action");
    }

    /// <summary>
    /// enemyStateがDamagedに切り替わった際に呼ばれる
    /// overrideする前提の関数
    /// </summary>
    virtual async protected UniTask OnDamagedActionAsync(CancellationToken token)
    {
        //確認用　後で消す
        Debug.Log($"Enemy:{this.gameObject.name} damaged！\nStart damaged action");

        await LMotion.Create(this.transform.position, this.transform.position + Vector3.up * 5, 1)
            .WithEase(Ease.InOutCubic).BindToPosition(this.transform).ToUniTask(token);
        Debug.Log($"Enemy:{this.gameObject.name} finish damaged action");
    }

    /// <summary>
    /// enemyStateがDeathに切り替わった際に呼ばれる
    /// overrideする前提の関数
    /// </summary>
    virtual async protected UniTask OnDeathActionAsync(CancellationToken token)
    {
        //確認用　後で消す
        Debug.Log($"Enemy:{this.gameObject.name} dead！\nStart dead action");

        LMotion.Create(this.transform.position, this.transform.position + this.transform.forward * -5, 0.5f)
            .WithEase(Ease.OutQuart).WithLoops(2, LoopType.Yoyo).BindToPosition(this.transform).ToUniTask(token);
        await LMotion.Create(this.transform.position, this.transform.position + this.transform.forward * -5, 1)
            .WithEase(Ease.InOutCubic).BindToPosition(this.transform).ToUniTask(token);
        Debug.Log($"Enemy:{this.gameObject.name} finish dead action");
    }

    /// <summary>
    /// プレイヤーで呼び出すやつ
    /// </summary>
    /// <param name="playerAbility">プレイヤーのステータス
    /// </param>
    public virtual void CaptureStatusSet(IPlayerAbility playerAbility)
    {
        playerAbility = new NoneAbility();
    }

    public bool CanInteract()
    {
        //こっちも無理やり参照　後で治す
        return FindAnyObjectByType<PlayerStatus>().GetComponent<PlayerStatus>().Ability is NoneAbility ? true : false;
    }

    public string GetInteractionMessage()
    {
        return "キャプチャー";
    }

    public void OnInteract(IInteractCallBackReceivable caller)
    {
        CaptureStatusSet(FindAnyObjectByType<PlayerStatus>().GetComponent<PlayerStatus>().Ability);
    }

#if UNITY_EDITOR
    [SerializeField] private bool _isViewLastTarget = false;

    [LabelText("テスト用ステート変更機能")]
    [Button]
    public void ChangeStateOnInspector(EnemyState state)
    { 
        ChangeEnemyStateAsync(state);
    }


    private void OnDrawGizmos()
    {
        if (_isViewLastTarget)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawCube(_lastTarget, Vector3.one);
        }
        Gizmos.color = Color.red;
        //BaseOffsetの位置
        Gizmos.DrawSphere(new Vector3(this.transform.position.x, _enemyBaseOffset, this.transform.position.z), 0.1f);

        Gizmos.color = Color.yellow;
        // 正面の視野ラインを描画
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * _searchablePlayerDistance);
        Gizmos.color = Color.blue;

        // 左側の視野ラインを描画
        Vector3 leftDirection = this.transform.rotation * Quaternion.Euler(0, -_fieldOfViewHalf, 0) * Vector3.forward;
        Gizmos.DrawLine(this.transform.position, this.transform.position + leftDirection * _searchablePlayerDistance);

        // 右側の視野ラインを描画
        Vector3 rightDirection = this.transform.rotation * Quaternion.Euler(0, _fieldOfViewHalf, 0) * Vector3.forward;
        Gizmos.DrawLine(this.transform.position, this.transform.position + rightDirection * _searchablePlayerDistance);
    }
#endif
}
