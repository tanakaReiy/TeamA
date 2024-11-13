using Alchemy.Inspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WallObject : MonoBehaviour, IInteractable
{
    [LabelText("壁が動く速度")]
    [SerializeField] private float _wallMoveSpeed;

    private WallGenerator _wallGenerator;
    private Transform _generatePosition;
    private float _destoryDistance;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
    }

    private void FixedUpdate()
    {
        if(CheckWallDistance())
        {
            AdvanceWall();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// ジェネレーターからの距離を測るメソッド
    /// </summary>
    private bool CheckWallDistance()
    {
        //距離の2乗を求める
        float distanceSqr = (_generatePosition.transform.position - this.transform.position).sqrMagnitude;

        //破壊距離の2乗
        float destoryDisSqr = _destoryDistance * _destoryDistance;

        if(distanceSqr <= destoryDisSqr)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void AdvanceWall()
    {
        // 前方向に向かって進む
        Vector3 newPosition = transform.position + transform.forward * _wallMoveSpeed * Time.fixedDeltaTime;
        _rigidbody.MovePosition(newPosition);
    }

    /// <summary>
    /// ジェネレーターを登録する処理
    /// </summary>
    /// <param name="wallGenerator">壁のジェネレーター</param>
    public void RegisterGenerator(WallGenerator wallGenerator)
    {
        _wallGenerator = wallGenerator;
        _generatePosition = _wallGenerator.GeneratePosition;
        _destoryDistance = _wallGenerator.DestroyObjectDistance;
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
        //Collapse();
    }
}
