using Alchemy.Inspector;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// エネミーの生成管理、初期化、を受け持つ
/// </summary>
public class EnemySpawnManager : MonoBehaviour
{
    [LabelText("このシーンで生成するエネミーのデータリスト")]
    [SerializeField] private List<EnemyData> _enemyData = new List<EnemyData>();
    /// <summary>　現在のエネミーの生成タイマーをまとめた配列　</summary>
    private float[] _spawnTimerArr = new float[0];
    /// <summary>　エネミーの生成回数をまとめた配列　</summary>
    private int[] _enemyGenerateCountArr = new int[0];
    /// <summary>　現在のシーン上のエネミーが存在するかをまとめた配列　</summary>
    private bool[] _isEnemyExistArr = new bool[0];

    [LabelText("エネミーのスポーン位置を表示するBoxの色")]
    [FoldoutGroup("Gizmos Settings")][SerializeField] private Color _spawnPointColor = new Color(1, 0, 0, 0.7f);
    [LabelText("エネミーのスポーン位置を表示するBoxの大きさ")]
    [FoldoutGroup("Gizmos Settings")][SerializeField] private Vector3 _spawnPointScale = Vector3.one;

    [LabelText("エネミーの移動目標をを表示するBoxの色")]
    [FoldoutGroup("Gizmos Settings")][SerializeField] private Color _targetPointColor = new Color(1, 0, 1, 0.5f);
    [LabelText("エネミーの移動目標を表示するBoxの大きさ")]
    [FoldoutGroup("Gizmos Settings")][SerializeField] private Vector3 _targetPointScale = Vector3.one;
    private void Start()
    {
        //もしエネミーデータが一つもない、もしくはEnemyPrefabLoaderが存在しなければ自身を削除
        if (_enemyData.Count == 0 || EnemyPrefabLoader.Instance == null)
        {
            this.gameObject.SetActive(false);
            return;
        }
        //それぞれ配列を初期化
        _spawnTimerArr = new float[_enemyData.Count];
        _enemyGenerateCountArr = new int[_enemyData.Count];
        _isEnemyExistArr = new bool[_enemyData.Count];
        //最初はすぐにスポーンするようにタイマーを初期化
        for (int i = 0; i < _enemyData.Count; i++)
        {
            _spawnTimerArr[i] = _enemyData[i].GenerateInterval - 1;
        }
    }

    private void Update()
    {
        for (int i = 0; i < _enemyData.Count; i++)
        {
            //タイマー加算
            _spawnTimerArr[i] += Time.deltaTime;
            if (IsSpawnableEnemy(i))
            {
                //実際にロードする
                GameObject? loadedObject = EnemyPrefabLoader.Instance.GetEnemyPrefab(_enemyData[i].Enemy);
                //nullでなければエネミーを初期化する
                if (loadedObject != null)
                {
                    InitializeSpawnedEnemy(loadedObject, i);
                }
            }
        }
    }
    /// <summary>
    /// エネミーがスポーンできるか判定
    /// </summary>
    private bool IsSpawnableEnemy(int index)
    {
        return _enemyData[index].SpawnablePlayerDistanceSquare >= (PlayerManager.Instance.transform.position - _enemyData[index].SpawnPoint).sqrMagnitude
                && !_isEnemyExistArr[index]
                && (_enemyData[index].MaxGenerateCnt <= -1  || _enemyGenerateCountArr[index] < _enemyData[index].MaxGenerateCnt )
                && _spawnTimerArr[index] >= _enemyData[index].GenerateInterval;
    }

    private void InitializeSpawnedEnemy(GameObject enemyObject, int index)
    {
        //エネミー生成
        GameObject instantiatedObject = GameObject.Instantiate(enemyObject);
        //スポーンに伴う値の変動
        _enemyGenerateCountArr[index]++;
        _isEnemyExistArr[index] = true;
        _spawnTimerArr[index] = 0;
        //エネミー内部の初期化
        instantiatedObject.transform.position = _enemyData[index].SpawnPoint;
        instantiatedObject.transform.rotation *= Quaternion.AngleAxis(_enemyData[index].SpawnedEnemyRotationY, Vector3.up);
        EnemyBase enemyBase = instantiatedObject.GetComponent<EnemyBase>();
        if (!enemyBase)
        {
            enemyBase = instantiatedObject.AddComponent<EnemyBase>();
        }
        //エネミーが破棄される際に行う処理を登録
        enemyBase._disposeAction += RegisterAction;
        enemyBase.GetNextPosition += _enemyData[0].MovePatern.GetNextTarget;
        enemyBase.GetNextGoalAction += _enemyData[0].MovePatern.NextTargetActionAsync;

        void RegisterAction()
        {
            _isEnemyExistArr[index] = false;
            _enemyData[index].MovePatern.Dispose(); 
            enemyBase.GetNextPosition -= _enemyData[0].MovePatern.GetNextTarget;
            enemyBase.GetNextGoalAction -= _enemyData[0].MovePatern.NextTargetActionAsync;
            enemyBase._disposeAction -= RegisterAction;
        }

        enemyBase.Initialize();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for (int i = 0; i < _enemyData.Count; i++)
        {
            if (_enemyData[i].IsDrawTargetsPosition)
            {
                //エネミーのスポーンポイントの描画
                Gizmos.color = _spawnPointColor;
                Vector3 centerSpawnPoint = _enemyData[i].SpawnPoint + Vector3.up * _spawnPointScale.y * 0.5f;
                Gizmos.DrawCube(centerSpawnPoint, _spawnPointScale);
                //エネミーの正面を描画
                Gizmos.DrawLine(centerSpawnPoint, centerSpawnPoint + Quaternion.AngleAxis(_enemyData[i].SpawnedEnemyRotationY, Vector3.up) * Vector3.forward);
                //nullチェック
                if (_enemyData[i].MovePatern == null) { continue; };
                //エネミーの巡回地の描画
                Gizmos.color = _targetPointColor;
                foreach ((Vector3 position, float rotationEuler) targetData in _enemyData[i].MovePatern.GetAllTargets())
                {
                    Vector3 centerTarget = targetData.position + Vector3.up * _targetPointScale.y * 0.5f;
                    Gizmos.DrawCube(centerTarget, _targetPointScale);
                    Gizmos.DrawLine(centerTarget, centerTarget + Quaternion.AngleAxis(targetData.rotationEuler, Vector3.up) * Vector3.forward);
                }
            }
        }
    }
#endif
}
