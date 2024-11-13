using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallObject : MonoBehaviour, IInteractable
{
    
    private void Start()
    {
        
    }

    ///// <summary>
    ///// 崩壊処理
    ///// </summary>
    //private void Collapse()
    //{
    //    //分割されたブロックのPrefabを生成する
    //    Transform breakBlockTransform = Instantiate(_breakBlockPrefab, transform.position, Quaternion.identity);

    //    //崩壊した各パーツに力を加える
    //    foreach (Rigidbody rigidbody in breakBlockTransform.GetComponentsInChildren<Rigidbody>())
    //    {
    //        rigidbody.AddExplosionForce(_collapseForce, transform.position + Vector3.up * 0.5f, _collapseRadius);
    //    }

    //    //崩壊したパーツを消す処理
    //    Destroy(breakBlockTransform.gameObject, _collapseDestorySecond);
    //    //元のブロックを削除
    //    Destroy(this.gameObject);
    //}
    public bool CanInteract()
    {
        throw new System.NotImplementedException();
    }

    public string GetInteractionMessage()
    {
        throw new System.NotImplementedException();
    }

    public void OnInteract(IInteractCallBackReceivable caller)
    {
        throw new System.NotImplementedException();
    }
}
