using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class TestSpawner : MonoBehaviour
{
    [SerializeField] List<EnemyMoveData> _enemyMoveData = new List<EnemyMoveData>();

    Vector3 _enableSpawnPoint = Vector3.zero;

    private void Start()
    {
        GameObject enemyObject = Instantiate(_enemyMoveData[0]._enemyPrefab, _enemyMoveData[0]._spawnPoint, this.gameObject.transform.rotation);
        EnemyStateChanger enemyBase = enemyObject.GetComponent<EnemyStateChanger>();
        if (!enemyBase)
        {
            enemyBase = enemyObject.AddComponent<EnemyStateChanger>();
        }
        enemyBase.GetNextPosition += _enemyMoveData[0]._movePatern.GetNextTarget;
        enemyBase.GetNextGoalAction += _enemyMoveData[0]._movePatern.NextTargetActionAsync;
        enemyBase.Initialize();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for (int i = 0; i < _enemyMoveData.Count; i++)
        {
            if (_enemyMoveData[i]._isDrawTargetsPosition)
            {
                Gizmos.color = new Color(1, 0, 0, 0.7f);
                Gizmos.DrawCube(_enemyMoveData[i]._spawnPoint, (Vector3.one + Vector3.up) * 0.8f); 
                Gizmos.color = new Color(1, 0, 1, 0.5f);
                foreach (var position in _enemyMoveData[i]._movePatern.GetAllTargets())
                {
                    Gizmos.DrawCube(position, Vector3.one * 0.8f);
                }
            }
        }
    }
#endif
}
