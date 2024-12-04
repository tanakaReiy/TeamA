using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public enum EnemyEnum
{
    Cake,
    Marshmallow,
    Gum,
    Jelly,
    Icecream,
    Max
}

public class EnemyPrefabLoader : MonoBehaviour
{
    static EnemyPrefabLoader _instance;
    public static EnemyPrefabLoader Instance => _instance;

    /// <summary>
    /// 存在するエネミーのプレハブへのパスを書き込む配列
    /// </summary>
    private string[] _addressableAssetsPath = new string[(int)EnemyEnum.Max]
    {
        "Assets/Prefabs/Enemy/Surtr.prefab",
        "",
        "",
        "",
        ""
    };
    private Dictionary<EnemyEnum, GameObject> _enemyPrefabDictionary = new Dictionary<EnemyEnum, GameObject>();

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private async void Start()
    {
        for (int i = 0; i < (int)EnemyEnum.Max; i++)
        {
            if (_addressableAssetsPath[i] != "")
            {
                _enemyPrefabDictionary.Add((EnemyEnum)Enum.ToObject(typeof(EnemyEnum), i), await Addressables.LoadAssetAsync<GameObject?>(_addressableAssetsPath[i]));
            }
        }
    }
    private void OnDisable()
    {
        foreach (GameObject enemyObject in _enemyPrefabDictionary.Values)
        {
            try
            {
                Addressables.Release(enemyObject);
            }
            catch
            {
                Debug.Log($"GameObject already released. Object Name:{enemyObject.name}");
            }
        }
    }
    /// <summary>
    /// エネミーのパスを元にオブジェクトの読み込みをする
    /// </summary>
    /// <returns>読み込みに成功したらオブジェクト、失敗したらnullが戻る</returns>
    public GameObject? GetEnemyPrefab(EnemyEnum enemy)
    {
        GameObject loadedObject = default;
        _enemyPrefabDictionary.TryGetValue(enemy, out loadedObject);
        if (loadedObject == default)
        {
            Debug.Log($"GameObject not Found. EnemyEnum:{enemy.ToString()}");
            return null;
        }
        else
        {
            Debug.Log($"GameObject Found. EnemyEnum:{enemy.ToString()}");
            return loadedObject;
        }
    }
}
