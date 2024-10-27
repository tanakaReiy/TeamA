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
    public UniTask NextTargetActionAsync(Quaternion rotation, Transform transform, CancellationToken token);
}