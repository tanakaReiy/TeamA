using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

[System.Serializable]
public class EnemyMoveData
{
    [LabelText("エネミーの巡回位置を表示するか")]
    public bool _isDrawTargetsPosition = false;

    [LabelText("エネミーのプレハブ")]
    public GameObject _enemyPrefab;

    [LabelText("エネミーのスポーン位置")]
    public Vector3 _spawnPoint = Vector3.zero;

    [LabelText("生成する回数 / -1を選ぶと無限に生成")]
    public int _mamGenerateCnt = 0;

    [LabelText("シーンに存在できる最大数")]
    public int _maxEnemyCnt = 0;

    [LabelText("エネミーの挙動")]
    [SerializeReference] public IMovePatternEnemy _movePatern;
}

public interface IMovePatternEnemy
{
    public Vector3[] GetAllTargets();
    public (Vector3 position, Vector3 direction) GetNextTarget();
    /// <summary>
    /// エネミーの次の目的地に移行する際の回転を制御する
    /// </summary>
    /// <param name="rotation">次の目的地にて設定されている向き</param>
    /// <param name="transform">エネミーのtransform</param>
    /// <param name="token">キャンセル用トークン</param>
    /// <returns></returns>
    public UniTask NextTargetActionAsync(Quaternion rotation, Transform transform, CancellationToken token);
}