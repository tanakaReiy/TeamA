using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    [SerializeField] private bool _isGizmoActive;
    [SerializeField] EnemyData _enemyData;
    int _existEnemyCnt = 0;
    [SerializeField] int _SpawnEnemyCnt = 0;
    int _SpawnEnemyCount = 0;

    Vector3 _enableSpawnPoint = Vector3.zero;

    private void Start()
    {
        GameObject enemyObject = Instantiate(_enemyData.EnemyPrefab, _enemyData.MovePatern.NextTarget().position, Quaternion.Euler(_enemyData.MovePatern.NextTarget().direction));
        EnemyBase enemyBase = enemyObject.GetComponent<EnemyBase>();
        if(!enemyBase)
        {
            enemyBase = enemyObject.AddComponent<EnemyBase>();
        }
        enemyBase.GetNextPosition += _enemyData.MovePatern.NextTarget;
        enemyBase.GetNextGoalAction += _enemyData.MovePatern.NextTargetActionAsync;
        enemyBase.Initialize();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(_isGizmoActive && _enemyData)
        {
            Gizmos.color = new Color(1, 0, 1, 0.5f);
            Gizmos.DrawCube(_enemyData.MovePatern.NextTarget().position, Vector3.one * 0.8f);
        }
    }
#endif
}
