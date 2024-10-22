using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    [SerializeField] EnemyData _enemyData;
    int _existEnemyCnt = 0;
    int _SpawnEnemyCnt = 0;


    private void Start()
    {
        GameObject enemyObject = Instantiate(_enemyData.EnemyPrefab);
        EnemyBase enemyBase = enemyObject.GetComponent<EnemyBase>();
        if(!enemyBase)
        {
            enemyBase = enemyObject.AddComponent<EnemyBase>();
        }
        enemyBase.GetNextPosition += _enemyData.MovePatern.NextTarget;
        enemyBase.Initialize();
    }
}
