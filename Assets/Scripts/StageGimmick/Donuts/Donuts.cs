using Alchemy.Inspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Donuts : MonoBehaviour
{
    [LabelText("加速度")]
    [SerializeField] private float _acceleration = 1.0f;
    [LabelText("最高速度")]
    [SerializeField] private float _maxSpeed = 10.0f;
    [LabelText("生存時間")]
    [SerializeField] private float _lifeTime = 5.0f;
    [LabelText("プレイヤーに対するあたり判定の半径")]
    [SerializeField] private float _radius = 5f;

    private Rigidbody _rigidbody;
    private float _timer;

    const int LayerMask = 1 << 7;

    private DonutsObjectPool _pool; //所属オブジェクトプール

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _pool = GameObject.FindObjectOfType<DonutsObjectPool>();
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if(Physics.SphereCast(this.transform.position, _radius, transform.forward,out hit, 0.1f, LayerMask, QueryTriggerInteraction.Ignore))
        {
            //プレイヤーに近づいた時の処理
            Debug.Log("Hit!!!!!");
        }
        //現在の速度
        Vector3 currentVelocity = _rigidbody.velocity;

        //最高速度未満の場合は加速させる
        if(currentVelocity.magnitude < _maxSpeed)
        {
            Vector3 force = transform.forward * _acceleration;
            _rigidbody.AddForce(force);
        }
        else
        {
            _rigidbody.velocity = currentVelocity.normalized * _maxSpeed;
        }

        //生存時間の管理
        _timer += Time.deltaTime;
        if(_timer >= _lifeTime)
        {
            _pool.ReturnToPool(this);
        }
    }

    public void ResetObject()
    {
        _timer = 0f;
        _rigidbody.velocity = Vector3.zero; // 速度リセット
        _rigidbody.angularVelocity = Vector3.zero; // 回転リセット
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position + Vector3.forward * 0.1f, _radius);
    }
}
