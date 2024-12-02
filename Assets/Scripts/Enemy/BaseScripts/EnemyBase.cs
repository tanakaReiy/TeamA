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
    public Func<(Vector3 position, float rotation)> GetNextPosition;

    public Func<Quaternion, Transform, CancellationToken, UniTask> GetNextGoalAction;

    //Playerの位置を知るための参照
    //後にシーンにあるエネミースポナーからPlayerのtransformを参照渡しする方法に変更
    //EnemyBaseから変更を加えない
    [ReadOnly]
    [SerializeField] public Transform _playerTransform;
    [ReadOnly]
    [SerializeField] private string _cueSheet = "CueSheet_0";
    [ReadOnly]
    [SerializeField] private float _currentHp;

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
    [SerializeField] private float _attackableDistance = 4;

    [LabelText("攻撃範囲")]
    [SerializeField] private Vector3 _attackArea = new Vector3(1, 1, 1);

    private Vector3 _lastTarget = Vector3.zero;

    private float _searchableTargetRange = 5;

    private int _onlyPlayerLayerInt = 0;
    private int _withoutPlayerLayerInt = 0;

    private bool _isTargetPlayer = false;
    private bool _isSearchPlayer = false;

    protected bool _isEnableDamageArea = false;
    /// <summary>
    /// 現在発生している攻撃がプレイヤーに当たったかを判定する
    /// </summary>
    private bool _isAttackDamagedPlayer;

    private CancellationTokenSource _cts;
    private CancellationToken _token;

    private bool _isTokenChecked = false;

    public Action _disposeAction;

    public void Initialize()
    {
        _cts = new CancellationTokenSource();
        _token = _cts.Token;

        _navMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
        GameObject playerObject;
        //初期化できたかどうかのnullチェック
        if (PlayerManager.Instance.TryGetPlayerRef(out playerObject) && _navMeshAgent && GetNextPosition != null && GetNextGoalAction != null && OnStart())
        {
            _initialized = true;
        }
        _playerTransform = playerObject.transform;

        _currentHp = _enemyHp;
        _navMeshAgent.agentTypeID = NavMesh.GetAreaFromName("EnemyMovable");

        _onlyPlayerLayerInt = 1 << 7;
        _withoutPlayerLayerInt = -1 - (1 << LayerMask.NameToLayer("Player"));

        _navMeshAgent.stoppingDistance = _stopDistance;
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
        if (!_initialized)
        {
            return;
        }

        if (_enemyState == EnemyState.Attack && _isEnableDamageArea)
        {
            DetectPlayer();
        }
        if (_navMeshAgent.isOnNavMesh && _enemyState == EnemyState.Move)
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
            if (_isTokenChecked)
            {
                //処理停止
                _cts.Cancel();
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
            _cts = new CancellationTokenSource();
            _token = _cts.Token;
            _isTokenChecked = true;
            try
            {
                //プレイヤーを見失った位置まで動く
                await UniTask.WaitUntil(() => Vector3.Distance(_navMeshAgent.destination, this.transform.position) <= _navMeshAgent.stoppingDistance
                , cancellationToken: _token);
                //辺りを一回見まわす
                await LookAround(_token);
            }
            catch
            {
                Debug.Log("SearchPlayer CancelThis\nWaitLastPlayerPositionAsync or LookAround");
            }
            _isTokenChecked = false;
            SetLastTarget();
        }
        //目的地との直線距離で判定している
        else if (Vector3.Distance(_navMeshAgent.destination, this.transform.position) <= _navMeshAgent.stoppingDistance && !_isTokenChecked)
        {
            _lastTarget = _navMeshAgent.destination;
            (Vector3 position, float rotation) NextTargetData = GetNextPosition();
            SetNextTarget(NextTargetData.Item1);

            //token再生成
            _cts = new CancellationTokenSource();
            _token = _cts.Token;

            _isTokenChecked = true;

            await GetNextGoalAction(Quaternion.AngleAxis(NextTargetData.rotation, Vector3.up), this.transform, _token);

            _isTokenChecked = false;

            //nullじゃなかったら明示的に解放
            if (_cts != null)
            {
                _cts.Dispose();
                Debug.Log("Dispose cts");
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
        bool playerVisible = Physics.Raycast(transform.position + Vector3.up * _navMeshAgent.baseOffset, toPlayerDirection, toPlayerDirection.magnitude + 1, _onlyPlayerLayerInt);
        bool noObstaclesExistPlayerDirection = !Physics.Raycast(transform.position + Vector3.up * _navMeshAgent.baseOffset, toPlayerDirection, _searchablePlayerDistance, _withoutPlayerLayerInt);

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
        catch
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
        _enemyState = enemyState;
        if (_isTokenChecked)
        {
            //処理停止
            _cts.Cancel();
        }
        if (_isTokenChecked)
        {
            //処理停止
            _cts.Cancel();
            //nullじゃなかったら明示的に解放
            if (_cts != null)
            {
                _cts.Dispose();
            }
        }
        //token再生成
        _cts = new CancellationTokenSource();
        _token = _cts.Token;

        //攻撃判定を出せるように初期化
        _isAttackDamagedPlayer = false;

        Debug.Log($"Enemy:{this.gameObject.name} change state {enemyState}");

        switch (enemyState)
        {
            case EnemyState.Attack:
                _navMeshAgent.enabled = false;
                _isTokenChecked = true;
                await OnAttackedActionAsync(_token);
                _isTokenChecked = false;
                _navMeshAgent.enabled = true;
                _navMeshAgent.destination = _lastTarget;

                ChangeEnemyStateAsync(EnemyState.Move);
                break;

            case EnemyState.Damage:
                _navMeshAgent.enabled = false;
                _isTokenChecked = true;
                await OnDamagedActionAsync(_token);
                _isTokenChecked = false;
                _navMeshAgent.enabled = true;
                _navMeshAgent.destination = _lastTarget;

                ChangeEnemyStateAsync(EnemyState.Move);
                break;

            case EnemyState.Death:
                GetComponent<Collider>().enabled = false;
                _navMeshAgent.enabled = false;
                _isTokenChecked = true;
                await OnDeathActionAsync(_token);
                _isTokenChecked = false;

                _initialized = false;
                Destroy(this.gameObject);
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
        try
        {
            //少し後退
            await LMotion.Create(this.transform.position, this.transform.position - this.transform.forward, 0.5f)
                .WithEase(Ease.InOutCubic).BindToPosition(this.transform).ToUniTask(token);
            //攻撃を有効化
            _isEnableDamageArea = true;
            /*//攻撃時のみプレイヤーとの判定を無くす
            Physics.IgnoreLayerCollision(this.gameObject.layer, _onlyPlayerLayerInt, true);*/
            //突進
            await LMotion.Create(this.transform.position, this.transform.position + this.transform.forward * 5, 1)
                .WithEase(Ease.InOutCubic).BindToPosition(this.transform).ToUniTask(token);
            //攻撃を無効化
            _isEnableDamageArea = false;
            /*//攻撃終了時にプレイヤーとの判定を戻す
            Physics.IgnoreLayerCollision(this.gameObject.layer, _onlyPlayerLayerInt, false);*/
            //突進終了後に後ろを振り向く
            await LMotion.Create(this.transform.rotation, Quaternion.LookRotation(-this.transform.forward, Vector3.up), 1)
                .WithEase(Ease.InOutQuad).BindToLocalRotation(transform).ToUniTask(token);
        }
        catch
        {
            Debug.Log($"Enemy:{this.gameObject.name} called Cancel [attacked action]");
        }

        Debug.Log($"Enemy:{this.gameObject.name} finish attacked action");
    }
    private void DetectPlayer()
    {
        if(_isAttackDamagedPlayer)
        {
            Debug.Log("This attack already damaged Player");
            return;
        }
        RaycastHit hit;
        if(Physics.BoxCast(transform.position + Vector3.up * _attackArea.y * 0.5f, new Vector3(_attackArea.x * 0.5f, _attackArea.y * 0.5f, 0.01f), transform.forward,
            out hit, this.transform.rotation, _attackArea.z, _onlyPlayerLayerInt))
        {
            try
            {
                if (hit.collider.gameObject.GetComponent<PlayerDamageReceiver>().ApplyDamage(1))
                {
                    _isAttackDamagedPlayer = true;
                    Debug.Log("Succeed ApplyDamage to Player");
                }
                else
                {
                    Debug.Log("Failed ApplyDamage to Player");
                }
            }
            catch
            {
                Debug.Log("Not apllied PlayerDamageReciever");
            }
        }
    }

    /// <summary>
    /// enemyStateがDamagedに切り替わった際に呼ばれる
    /// overrideする前提の関数
    /// </summary>
    virtual async protected UniTask OnDamagedActionAsync(CancellationToken token)
    {
        Debug.Log($"Enemy:{this.gameObject.name} damaged！\nStart damaged action");


        try
        {
            await LMotion.Create(this.transform.position, this.transform.position + Vector3.up * 5, 1)
                .WithEase(Ease.OutCubic).BindToPosition(this.transform).ToUniTask(token);
            await LMotion.Create(this.transform.position, this.transform.position - Vector3.up * 5, 1)
                .WithEase(Ease.InCubic).BindToPosition(this.transform).ToUniTask(token);
        }
        catch
        {
            Debug.Log($"Enemy:{this.gameObject.name} Call Cancel [damaged action]");
        }

        Debug.Log($"Enemy:{this.gameObject.name} finish damaged action");
    }

    /// <summary>
    /// enemyStateがDeathに切り替わった際に呼ばれる
    /// overrideする前提の関数
    /// </summary>
    virtual async protected UniTask OnDeathActionAsync(CancellationToken token)
    {
        Debug.Log($"Enemy:{this.gameObject.name} dead！\nStart dead action");
        try
        {
            await LMotion.Create(this.transform.position, this.transform.position + this.transform.forward * -5, 1)
                .WithEase(Ease.InOutCubic).BindToPosition(this.transform).ToUniTask(token);
            var material = this.GetComponent<MeshRenderer>().material;
            await LMotion.Create(this.transform.localScale, Vector3.zero, 1)
                .WithEase(Ease.OutCirc).BindToLocalScale(this.transform).ToUniTask(token);
        }
        catch
        {
            Debug.Log($"Enemy:{this.gameObject.name} Call Cancel [dead action]");
        }

        Debug.Log($"Enemy:{this.gameObject.name} finish dead action");
    }
    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="arg"></param>
    /// <returns>ダメージが実際に与えられたかを返す</returns>
    public bool ApplyDamage(float damage, IDamageArg arg = null)
    {
        if (_enemyState == EnemyState.Damage || _enemyState == EnemyState.Death)
        {
            Debug.Log($"Now, enemy:{this.gameObject.name} cant damaged");
            return false;
        }
        _currentHp -= damage;
        if (_currentHp <= 0)
        {
            _currentHp = 0;
            ChangeEnemyStateAsync(EnemyState.Death);
        }
        else
        {
            //CRIAudioManager.SE.Play3D(Vector3.zero, _cueSheet, "ダメージ音CueName");
            ChangeEnemyStateAsync(EnemyState.Damage);
        }
        return true;
    }

    private void OnDisable()
    {
        //ctsの解放
        if (_isTokenChecked || _isTokenChecked)
        {
            //処理停止
            _cts.Cancel();
            //nullじゃなかったら明示的に解放
            if (_cts != null)
            {
                _cts.Dispose();
            }
        }
        //その他登録されたアクションを実行
        //基本的に登録されたアクション内でこのActionへの登録も解放される
        _disposeAction?.Invoke();
        if(_disposeAction != null)
        {
            _disposeAction = null;
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
            if (_initialized)
            {
                //DrawNowTarget
                Gizmos.color = new Color(0, 0, 0, 0.6f);
                Gizmos.DrawCube(_navMeshAgent.destination, Vector3.one * 0.5f + Vector3.up * 3);
            }
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

        //攻撃判定を描画
        if(_isEnableDamageArea)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.gray;
        }
        var cache = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(transform.position + Vector3.up * _attackArea.y * 0.5f, transform.rotation, transform.lossyScale);
        Gizmos.DrawWireCube(Vector3.forward * _attackArea.z * 0.5f, _attackArea);
        Gizmos.matrix = cache;
    }

#endif
}
