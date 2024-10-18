using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBase : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private NavMeshHit _navMeshHit;

    /// <summary>
    /// 次の目的地をセットするFunc
    /// EnemySpawnerから個別に設定
    /// </summary>
    public Func<Vector3> GetNextPosition;

    //Playerの位置を知るための参照
    //後にシーンにあるエネミースポナーからPlayerのtransformを参照渡しする方法に変更
    //EnemyBaseから値を変更しない
    public Transform _playerTransform;

    private bool _initialized = false;

    [Title("基本設定")]

    [LabelText("HP")]
    [SerializeField] private int _hp = 1;

    [LabelText("現在の状態")]
    [SerializeField] private EnemyState _enemyState = EnemyState.Move;

    [LabelText("プレイヤーを発見できる距離")]
    [SerializeField] private float _searchablePlayerDistance = 5;

    [LabelText("プレイヤーを視野できる角度(正面からの角度)")]
    [SerializeField] private float _fieldOfViewHalf = 90;

    private Vector3 _lastTarget = Vector3.zero;

    private float _searchableTargetRange = 5;

    private bool _isTargetPlayer = false;
    private bool _isSearchPlayer = false;

    private CancellationTokenSource _cts;
    private CancellationToken _token;

    private void Start()
    {
        _cts = new CancellationTokenSource();
        _token = _cts.Token;

        _navMeshAgent = GetComponent<NavMeshAgent>();

        //memo
        //ここのプレイヤーのポジション参照の取得は後にシングルトンかスポナーから与えられる形に変わる
        _playerTransform = FindAnyObjectByType<PlayerInEnemyTest>().GetComponent<Transform>();

        if (_playerTransform && _navMeshAgent)
        {
            _initialized = true;
        }
    }
    private void FixedUpdate()
    {
        if (!_initialized || !_navMeshAgent.isOnNavMesh)
        {
            return;
        }

        if (_enemyState == EnemyState.Move)
        {
            SetMovePosition();
        }
    }
    private void SetMovePosition()
    {
        _isSearchPlayer = SearchPlayer();

        if (_isSearchPlayer)
        {
            _isTargetPlayer = true;
            ChasePlayer();
        }
        else if (_isTargetPlayer && !_isSearchPlayer)
        {
            _isTargetPlayer = false;
            SetLastTarget();
        }
        else if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            SetNextTarget();
        }
    }

    private bool SearchPlayer()
    {
        Vector3 toPlayerDirection = _playerTransform.position - transform.position;
        if (toPlayerDirection.magnitude < _searchablePlayerDistance
            && Vector3.Angle(transform.forward, toPlayerDirection) < _fieldOfViewHalf
            && !Physics.Raycast(transform.position, toPlayerDirection, _searchablePlayerDistance, -1 - 1 << LayerMask.NameToLayer("Player")))
        {
            return true;
        }
        return false;
    }

    private void ChasePlayer()
    {
        NavMesh.SamplePosition(_playerTransform.position, out _navMeshHit, _searchableTargetRange, 1);
        _navMeshAgent.destination = _navMeshHit.position;
    }

    private void SetLastTarget()
    {
        NavMesh.SamplePosition(_lastTarget, out _navMeshHit, _searchableTargetRange, 1);
        _navMeshAgent.destination = _navMeshHit.position;
    }

    private void SetNextTarget()
    {
        if (_navMeshAgent.destination != null)
        {
            _lastTarget = _navMeshAgent.destination;
        }
        Vector3 nextPosition = GetNextPosition();

        if (nextPosition != null)
        {
            NavMesh.SamplePosition(nextPosition, out _navMeshHit, _searchableTargetRange, 1);
            _navMeshAgent.destination = _navMeshHit.position;
        }
    }

    /// <summary>
    /// エネミーの状態を変更し、敵の状態に伴ったメソッドを1回呼び出す
    /// </summary>
    /// <param name="changedEnemyState"></param>
    private async void ChangeEnemyState(EnemyState changedEnemyState)
    {
        // すでにキャンセルされているなら例外を投げる
        _token.ThrowIfCancellationRequested();

        //処理停止
        _cts.Cancel();

        //token再生成
        _cts = new CancellationTokenSource();
        _token = _cts.Token;

        _enemyState = changedEnemyState;

        //確認用　後で消す
        Debug.Log($"Enemy:{this.gameObject.name} change state {_enemyState}");

        switch (_enemyState)
        {
            case EnemyState.Attack:
                OnAttackedActionAsync(_token);
                break;
            case EnemyState.Damage:
                OnDamagedActionAsync(_token);
                break;
            case EnemyState.Death:
                OnDeathActionAsync(_token);
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
        _hp -= damage;
        if (_hp < 0)
        {
            _hp = 0;
            ChangeEnemyState(EnemyState.Death);
        }
        else
        {
            ChangeEnemyState(EnemyState.Damage);
        }
    }

    /// <summary>
    /// enemyStateがAttackに切り替わった際に呼ばれる
    /// overrideする前提の関数
    /// </summary>
    protected virtual async UniTask OnAttackedActionAsync(CancellationToken cancellationToken)
    {
        //確認用　後で消す
        Debug.Log($"Enemy:{this.gameObject.name} attacked！");
    }

    /// <summary>
    /// enemyStateがDamagedに切り替わった際に呼ばれる
    /// overrideする前提の関数
    /// </summary>
    protected virtual async UniTask OnDamagedActionAsync(CancellationToken cancellationToken)
    {
        //確認用　後で消す
        Debug.Log($"Enemy:{this.gameObject.name} damaged！");
    }

    /// <summary>
    /// enemyStateがDeathに切り替わった際に呼ばれる
    /// overrideする前提の関数
    /// </summary>
    protected virtual async UniTask OnDeathActionAsync(CancellationToken cancellationToken)
    {
        //確認用　後で消す
        Debug.Log($"Enemy:{this.gameObject.name} dead！");
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        // 正面の視野ラインを描画
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * _searchablePlayerDistance);
        Gizmos.color = Color.blue;

        // 左側の視野ラインを描画
        Vector3 leftDirection = transform.rotation * Quaternion.Euler(0, -_fieldOfViewHalf, 0) * Vector3.forward;
        Gizmos.DrawLine(transform.position, transform.position + leftDirection * _searchablePlayerDistance);

        // 右側の視野ラインを描画
        Vector3 rightDirection = transform.rotation * Quaternion.Euler(0, _fieldOfViewHalf, 0) * Vector3.forward;
        Gizmos.DrawLine(transform.position, transform.position + rightDirection * _searchablePlayerDistance);
    }
#endif
}
