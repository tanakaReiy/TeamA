using Alchemy.Inspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    [LabelText("壁のプレハブ")]
    [SerializeField] private List<WallObject> _wallObjectsPrefab;
    [LabelText("生成するプレハブの重み")]
    [SerializeField] private List<float> _wallObjectWeight;
    [LabelText("生成位置")]
    [SerializeField] private Transform _generatePosition;

    public Transform GeneratePosition
    {
        get { return _generatePosition; }
    }

    [LabelText("生成間隔")]
    [SerializeField] private float _generateInterval;
    [LabelText("生成したオブジェクトを消す距離")]
    [SerializeField] private float _destroyObjectDistance;

    public float DestroyObjectDistance  // publicプロパティ（読み取り専用）
    {
        get { return _destroyObjectDistance; }
    }
    private bool _isGenerate; //生成スタートする処理
    private float _timer;

    private void Start()
    {
        _isGenerate = false;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
       if ( _wallObjectsPrefab != null && _isGenerate && _timer >= _generateInterval)
       {
            RandomWallGenerate();
            _timer = 0;
       }
    }

    // 壁を生成する処理
    private void RandomWallGenerate()
    {
        int objIndex = GetRandomItem();
        if( objIndex != -1 )
        {
            WallObject wallObject = Instantiate(_wallObjectsPrefab[objIndex], _generatePosition.position, transform.rotation);
            wallObject.RegisterGenerator(this);
            Debug.Log("生成しました");
        }
        else
        {
            Debug.LogWarning("生成に失敗しました");
        }
    }

    // 壁のブレハブの添え字を返すメソッド
    private int GetRandomItem()
    {
        float totalWeight = 0;

        foreach (var weight in _wallObjectWeight)
        {
            totalWeight += weight;
        }

        float randomValue = UnityEngine.Random.Range(0, totalWeight);
        float cumulativeWeight = 0;

        for (int i = 0; i < _wallObjectsPrefab.Count; i++)
        {
            cumulativeWeight += _wallObjectWeight[i];
            if (randomValue <= cumulativeWeight)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// 生成フラグをオンにする
    /// </summary>
    [Button]
    public void OnGenerateFlag()
    {
        _isGenerate = true;
    }

    /// <summary>
    /// 生成フラグをオフにする
    /// </summary>
    [Button]
    public void OffGenerateFlag()
    {
        _isGenerate = false;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying) return;

        Gizmos.color = Color.red;
        Gizmos.DrawCube(_generatePosition.position, this.transform.localScale);

        Gizmos.color = Color.blue;
        // 回転と位置を反映させるためにGizmos.matrixを設定
        Matrix4x4 cubeMatrix = Matrix4x4.TRS(
            _generatePosition.position + _generatePosition.forward * _destroyObjectDistance,
            _generatePosition.rotation, // 回転を適用
            Vector3.one // スケールはデフォルトのまま
        );

        Gizmos.matrix = cubeMatrix;
        Gizmos.DrawCube(Vector3.zero, new Vector3(10f, 5f, 1f));

        Gizmos.matrix = Matrix4x4.identity;
    }
}
