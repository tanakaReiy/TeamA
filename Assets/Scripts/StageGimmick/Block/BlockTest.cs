using Alchemy.Inspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マシュマロブロック
/// </summary>
public class BlockTest : MonoBehaviour, IInteractable
{
    [SerializeField, LabelText("ブロックが壊せるかのフラグ")] private bool _isBreak;
    [SerializeField, LabelText("崩れるプロックのプレハブ")] private Transform _breakBlockPrefab;
    [SerializeField, LabelText("崩壊時の力")] private float _collapseForce = 150f;
    [SerializeField, LabelText("崩壊の影響範囲")] private float _collapseRadius = 5f;
    [SerializeField, LabelText("崩壊したパーツを消す秒数")] private float _collapseDestorySecond = 5f;


    public bool CanInteract()
    {
        return true;
    }

    public string GetInteractionMessage()
    {
        return "ブロック";
    }

    public void OnInteract(IInteractCallBackReceivable caller)
    {
        //ここにプレイヤーの状態を入れる
        if (_isBreak)
        {
            Collapse();
        }
        else
        {
            Debug.Log("破壊不可");
        }
    }

    /// <summary>
    /// 崩壊処理
    /// </summary>
    private void Collapse()
    {
        //分割されたブロックのPrefabを生成する
        Transform breakBlockTransform = Instantiate(_breakBlockPrefab, transform.position, Quaternion.identity);

        //崩壊した各パーツに力を加える
        foreach (Rigidbody rigidbody in breakBlockTransform.GetComponentsInChildren<Rigidbody>())
        {
            rigidbody.AddExplosionForce(_collapseForce, transform.position + Vector3.up * 0.5f, _collapseRadius);
        }

        //崩壊したパーツを消す処理
        Destroy(breakBlockTransform.gameObject, _collapseDestorySecond);
        //元のブロックを削除
        Destroy(this.gameObject);
    }

    /// <summary>
    /// ボタンテスト用
    /// </summary>
    public void OnButton()
    {
        OnInteract(null);
    }
}
