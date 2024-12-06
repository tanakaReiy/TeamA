using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    [SerializeField] List<EnemyData> _enemyData = new List<EnemyData>();

    Vector3 _enableSpawnPoint = Vector3.zero;

    private void Start()
    {
        GameObject enemyObject = Instantiate(_enemyData[0].EnemyPrefab, _enemyData[0].SpawnPoint, this.gameObject.transform.rotation);
        EnemyBase enemyBase = enemyObject.GetComponent<EnemyBase>();
        if (!enemyBase)
        {
            enemyBase = enemyObject.AddComponent<EnemyBase>();
        }
        enemyBase.GetNextPosition += _enemyData[0].MovePatern.GetNextTarget;
        enemyBase.GetNextGoalAction += _enemyData[0].MovePatern.NextTargetActionAsync;
        enemyBase.Initialize();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for (int i = 0; i < _enemyData.Count; i++)
        {
            if (_enemyData[i].IsDrawTargetsPosition)
            {
                Gizmos.color = new Color(1, 0, 0, 0.7f);
                Gizmos.DrawCube(_enemyData[i].SpawnPoint, (Vector3.one + Vector3.up) * 0.8f);
                Gizmos.color = new Color(1, 0, 1, 0.5f);
                foreach (var data in _enemyData[i].MovePatern.GetAllTargets())
                {
                    Gizmos.DrawCube(data.position, Vector3.one * 0.8f);
                }
            }
        }
    }
#endif
}
