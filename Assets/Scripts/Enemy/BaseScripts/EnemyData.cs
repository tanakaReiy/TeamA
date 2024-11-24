using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
#if UNITY_EDITOR
    [LabelText("エネミーの巡回位置を表示するか")]
    public bool IsDrawTargetsPosition = false;
#endif
    /// <summary>
    /// ※旧形式のためEnumを使用した方式に完全に移行した後削除されます
    /// </summary>
    [LabelText("エネミーのプレハブ")]
    public GameObject EnemyPrefab;

    [LabelText("エネミーの種類")]
    public EnemyEnum Enemy;

    [LabelText("エネミーのスポーン位置")]
    public Vector3 SpawnPoint = Vector3.zero;

    [LabelText("エネミーのスポーン時の向き")]
    public float SpawnedEnemyRotationY = 0;

    [LabelText("生成する回数 / -1を選ぶと無限に生成")]
    public int MaxGenerateCnt = 0;

    [LabelText("生成にかかる時間")]
    public float GenerateInterval = 5;

    [LabelText("生成可能なプレイヤーとの距離の二乗")]
    public float SpawnablePlayerDistanceSquare = 256;

    [LabelText("エネミーの挙動")]
    [SerializeReference] public IMovePatternEnemy MovePatern;
}

public interface IMovePatternEnemy
{
    public (Vector3 position, float direction)[] GetAllTargets();
    public (Vector3 position, float direction) GetNextTarget();

    public void OnDisposed();
    /// <summary>
    /// エネミーの次の目的地に移行する際の回転を制御する
    /// </summary>
    /// <param name="rotation">次の目的地にて設定されている向き</param>
    /// <param name="transform">エネミーのtransform</param>
    /// <param name="token">キャンセル用トークン</param>
    /// <returns></returns>
    public UniTask NextTargetActionAsync(Quaternion rotation, Transform transform, CancellationToken token);
}