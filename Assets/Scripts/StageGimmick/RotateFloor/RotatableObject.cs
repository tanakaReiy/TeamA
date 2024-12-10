using Alchemy.Inspector;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RotatableObject : MonoBehaviour
{
    [SerializeField] public RotateFloorData Data;
    [SerializeField] private float _adjustOverlapBoxRange = 0.1f;
    private int _obstacleOrRotateLayer;
    private int _floorLayer;
    private void Start()
    {
        _obstacleOrRotateLayer = 1 << 10;
        _floorLayer = 1 << 11;
        Data.Floor = this.gameObject;
        Data.Collider = this.gameObject.GetComponent<Collider>();
        Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }
    public void CheckObstacle(float rotateInput)
    {
        if(Physics.OverlapBox(this.transform.position,
            this.transform.localScale * (0.5f + _adjustOverlapBoxRange),
            transform.rotation, _obstacleOrRotateLayer, QueryTriggerInteraction.Collide).Length > 0)
        {
            Data.IsRotatable = false;
            if(rotateInput != 0)
            {
                Data.ObstacledInput = rotateInput > 0 ? 1 : -1;
            }
        }
    }
    public void CheckRotateFloor()
    {
        if (Physics.OverlapBox(this.transform.position,
            this.transform.localScale * (0.5f + _adjustOverlapBoxRange),
            transform.rotation, _floorLayer, QueryTriggerInteraction.Collide).Length > 1)
        {
            Data.IsRotatable = true;
        }
    }
    /// <summary>
    /// 回せない状態かつ入力が前回止まっていた入力であるならfalse、それ以外ならtrueを返す
    /// </summary>
    /// <param name="rotateInput">
    /// 回転用の入力
    /// </param>
    public bool IsRotatable(float rotateInput) 
    {
        if(!Data.IsRotatable && (rotateInput - Data.ObstacledInput) * (rotateInput - Data.ObstacledInput) < 2)
        {
            return false;
        }
        return true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var normalMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, transform.localScale * (1 + _adjustOverlapBoxRange));
        Gizmos.matrix = normalMatrix;
    }
}

[System.Serializable]
public class RotateFloorData
{
    [Alchemy.Inspector.ReadOnly]
    public GameObject Floor;
    [Alchemy.Inspector.ReadOnly]
    public Collider Collider;
    public bool IsRotatable =　false;
    public float ObstacledInput = 0;
    public float Angle;
    public Vector3 StickChildVector;
    public RotateFloorData ChildData;
}