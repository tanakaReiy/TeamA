using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    [LabelText("エネミーのプレハブ")]
    [SerializeField] private GameObject _enemyPrefab;

    public GameObject EnemyPrefab => _enemyPrefab;

    [LabelText("生成する回数 / -1を選ぶと無限に生成")]
    [SerializeField] private int _mamGenerateCnt = 0;
    public int MaxGenerateCnt => _mamGenerateCnt;

    [LabelText("シーンに存在できる最大数")]
    [SerializeField] private int _maxEnemyCnt = 0;
    public int MaxEnemyCnt => _maxEnemyCnt;

    [LabelText("エネミーの挙動")]
    [SerializeField, SerializeReference] private IMovePatternEnemy _movePatern;
    public IMovePatternEnemy MovePatern => _movePatern;
}

public interface IMovePatternEnemy 
{
    public (Vector3 position, Vector3 direction) NextTarget();
    /// <summary>
    /// エネミーの次の目的地に移行する際の回転を制御する
    /// </summary>
    /// <param name="rotation">次の目的地にて設定されている向き</param>
    /// <param name="transform">エネミーのtransform</param>
    /// <param name="token">キャンセル用トークン</param>
    /// <returns></returns>
    public UniTask NextTargetActionAsync(Quaternion rotation, Transform transform, CancellationToken token);
}