using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Iinitialzableを持つゲームオブジェクトをここに登録するとStartで初期化されます
/// </summary>
public class Initializer : MonoBehaviour
{
    [SerializeField] private List<GameObject> _initializeObjects;

    private void Start()
    {
        for (int i = 0; i < _initializeObjects.Count; i++)
        {
            IInitializable[] inits = _initializeObjects[i].GetComponents<IInitializable>();
            if(inits == null) { return; }
            for(int k = 0; k < inits.Length; k++)
            {
                inits[k].Initialize();
            }
        }
    }
}

/// <summary>
/// 非アクティブでも初期化を実行したいObjectのためのインターフェイスです
/// </summary>
public interface IInitializable
{
    void Initialize();
}