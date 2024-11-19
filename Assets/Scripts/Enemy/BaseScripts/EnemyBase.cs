using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using DamageSystem;
using LitMotion;
using LitMotion.Extensions;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBase : MonoBehaviour , IDamagable
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

    [LabelText("現在の状態")]
    [SerializeField] private EnemyState _enemyState = EnemyState.Move;

    [LabelText("設定するHP")]
    [SerializeField] private float _enemyHp = 1;

    [LabelText("設定する上限の移動速度")]
    [SerializeField] private float _enemyMaxSpeed = 6;

    [LabelText("設定する目標に対して止まる距離")]
    [SerializeField] private float _stopDistance = 1;

    [LabelText("設定する回転速度（deg/s）")]
    [SerializeField] private float _enemyAngularSpeed = 240;

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


        //ここのプレイヤーのポジション参照の取得は後にシングルトンかスポナーから与えられる形に変わる
        _playerTransform = FindAnyObjectByType<PlayerInEnemyTest>().GetComponent<Transform>();

        //初期化できたかどうかのnullチェック
        if (_playerTransform && _navMeshAgent && GetNextPosition != null && GetNextGoalAction != null && OnStart())
        {
            _initialized = true;
        }

        _navMeshAgent.agentTypeID = NavMesh.GetAreaFromName("EnemyMovable");

        _navMeshAgent.stoppingDistance = 1;
        _navMeshAgent.speed = _enemyMaxSpeed;
        _navMeshAgent.angularSpeed = _enemyAngularSpeed;
        NavMesh.SamplePosition(GetNextPosition().position, out _navMeshHit, _searchableTargetRange, 1);
        _navMeshAgent.destination = _navMeshHit.position;
    }
    /// <summary>
    /// 継承先で、本来Start()でしたい処理をここに書く
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
            try
            {
                //プレイヤーを見失った位置まで動く
                await UniTask.WaitUntil(() => Vector3.Distance(_navMeshAgent.destination, this.transform.position) <= _navMeshAgent.stoppingDistance
                , cancellationToken: _cancellTokenMovedAction);
                //辺りを一回見まわす
                await LookAround(_cancellTokenMovedAction);
            }
            catch (OperationCanceledException e)
            {
                Debug.Log("SearchPlayer CancelThis\nWaitLastPlayerPositionAsync or LookAround");
            }


            _isPlayMovedAction = false;
            SetLastTarget();


        }
        //目的地との直線距離で判定している
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

        bool seachablePlayerDistance = toPlayerDirection.magnitude < _searchablePlayerDistance;
        bool playerInSight = Vector3.Angle(transform.forward, toPlayerDirection) < _fieldOfViewHalf;
        bool playerVisible = !Physics.Raycast(transform.position, toPlayerDirection, toPlayerDirection.magnitude, -1 - (1 << LayerMask.NameToLayer("Player")));
        bool noObstaclesExistPlayerDirection = Physics.Raycast(transform.position, toPlayerDirection, _searchablePlayerDistance, (int)Mathf.Pow(2, 7));

        if (seachablePlayerDistance && playerInSight && playerVisible && noObstaclesExistPlayerDirection)
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
        if (!_isTargetPlayer)
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
        try
        {
            Debug.Log("見失い挙動開始");
            //２秒で時計回りに75度回転
            await LMotion.Create(this.transform.rotation, this.transform.rotation * Quaternion.AngleAxis(60, Vector3.up), 2)
                .WithEase(Ease.InOutCubic).BindToLocalRotation(transform).ToUniTask(token);
            //２秒で反時計回りに130度回転
            await LMotion.Create(this.transform.rotation, this.transform.rotation * Quaternion.AngleAxis(-130, Vector3.up), 2)
                .WithEase(Ease.InOutCubic).BindToLocalRotation(transform).ToUniTask(token);
            Debug.Log("見失い挙動終了");
        }
        catch (OperationCanceledException e)
        {
            Debug.Log("Cancel LookAround");
        }
    }

    /// <summary>
    /// エネミーの状態を変更し、敵の状態に伴ったメソッドを1回呼び出す
    /// </summary>
    /// <param name="enemyState"></param>
    async protected void ChangeEnemyStateAsync(EnemyState enemyState)
    {
        if (_isPlayMovedAction)
        {
            //処理停止
            _ctsMovedAction.Cancel();
        }
        if (_isPlayChangeStateAction)
        {
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

        Debug.Log($"Enemy:{this.gameObject.name} change state {enemyState}");

        switch (enemyState)
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
    /// enemyStateがAttackに切り替わった際に呼ばれる
    /// overrideする前提の関数
    /// </summary>
    virtual async protected UniTask OnAttackedActionAsync(CancellationToken token)
    {
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
        Debug.Log($"Enemy:{this.gameObject.name} dead！\nStart dead action");

        await LMotion.Create(this.transform.position, this.transform.position + this.transform.forward * -5, 1)
            .WithEase(Ease.InOutCubic).BindToPosition(this.transform).ToUniTask(token);

        Debug.Log($"Enemy:{this.gameObject.name} finish dead action");
    }

    /// <summary>
    /// Capturableに付ける用の関数
    /// </summary>
    public void OnCapturedAction()
    {
        ChangeEnemyStateAsync(EnemyState.Idle);
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="arg"></param>
    public void ApplyDamage(float damage, IDamageArg arg = null)
    {
        if (_enemyState == EnemyState.Damage || _enemyState == EnemyState.Death)
        {
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

    private void OnDisable()
    {
        if (_isPlayMovedAction)
        {
            //処理停止
            _ctsMovedAction.Cancel();
            //nullじゃなかったら明示的に解放
            if (_ctsMovedAction != null)
            {
                _ctsMovedAction.Dispose();
            }
        }
        if (_isPlayChangeStateAction)
        {
            //処理停止
            _ctsChangeStateAction.Cancel();
            //nullじゃなかったら明示的に解放
            if (_ctsChangeStateAction != null)
            {
                _ctsChangeStateAction.Dispose();
            }
        }
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
            //DrawLastTarget
            Gizmos.color = Color.cyan;
            Gizmos.DrawCube(_lastTarget, Vector3.one);
            if (!_initialized)
                return;
            //DrawNowTarget
            Gizmos.color = new Color(0, 0, 0, 0.6f);
            Gizmos.DrawCube(_navMeshAgent.destination, Vector3.one * 0.5f + Vector3.up * 3);
        }
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
