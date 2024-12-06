using Alchemy.Inspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutsGenerator : MonoBehaviour
{
    [LabelText("生成間隔")]
    [SerializeField] private float _generateInterval;
    [LabelText("生成位置")]
    [SerializeField] private Transform _generatePosition;
    private DonutsObjectPool _pool;
    private float _timer = 0;

    private void Start()
    {
        _pool = GameObject.FindObjectOfType<DonutsObjectPool>();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if( _timer > _generateInterval )
        {
            DonutsGenerate();
            _timer = 0;
        }
    }

    private void DonutsGenerate()
    {
        // プールからオブジェクトを取得
        Donuts obj = _pool.Get();

        // 位置と回転をジェネレーターに合わせる
        obj.transform.position = _generatePosition.position;
        obj.transform.rotation = _generatePosition.rotation;

        obj.ResetObject();
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying) return;

        Gizmos.color = Color.red;
        Gizmos.DrawCube(_generatePosition.position, this.transform.localScale);
    }
}
