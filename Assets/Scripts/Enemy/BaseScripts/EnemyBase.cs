using Alchemy.Inspector;
using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBase : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private NavMeshHit _navMeshHit;

    /// <summary>
    /// 次の目的地をセットするアクション
    /// </summary>
    public Action MoveEnemy;

    //後にシーンにあるエネミースポナーからPlayerのtransformを参照渡しする方法に変更
    //EnemyBaseから値を変更しない
    private Transform _playerTransform;

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

    private Transform _lastTarget;

    private bool _isTargetPlayer = false;
    private bool _isSearchPlayer = false;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _playerTransform = FindAnyObjectByType<PlayerInEnemyTest>().GetComponent<Transform>();
        if (_playerTransform && _navMeshAgent)
        {
            _initialized = true;
        }
    }
    private void Update()
    {
        if (!_initialized || !_navMeshAgent.isOnNavMesh)
        {
            return;
        }

        if (_enemyState == EnemyState.Move)
        {
            StateMove();
        }
    }
    void StateMove()
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
            //SetLastTarget
        }
        else if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            //SetNextTarget
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
        NavMesh.SamplePosition(_playerTransform.position, out _navMeshHit, 5, 1);
        _navMeshAgent.destination = _navMeshHit.position;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
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
