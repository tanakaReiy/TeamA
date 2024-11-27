using Alchemy.Inspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPoolBase<T> : MonoBehaviour where T : Component
{
    [LabelText("プールするオブジェクトプレハブ")]
    [SerializeField] protected T _objectPrefab;
    [LabelText("初期プール数")]
    [SerializeField] protected int _initialSize = 10;

    private Queue<T> _pool;

    protected virtual void Awake()
    {
        _pool = new Queue<T>();

        //初期プールを作成
        for (int i = 0; i < _initialSize; i++)
        {
            T obj = CreateNewObject();
            _pool.Enqueue(obj);
        }
    }

    // 新しいオブジェクトを作成
    private T CreateNewObject()
    {
        T obj = Instantiate(_objectPrefab);
        obj.gameObject.SetActive(false);
        return obj;
    }

    // オブジェクトを取得
    public T Get()
    {
        if (_pool.Count > 0)
        {
            T obj = _pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }

        // プールが空の場合、新規作成
        return CreateNewObject();
    }

    // オブジェクトをプールに戻す
    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
    }
}
