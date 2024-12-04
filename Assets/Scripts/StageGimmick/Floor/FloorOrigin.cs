using Alchemy.Inspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorOrigin : MonoBehaviour
{
    [LabelText("��]���x")]
    [SerializeField] private float _rotateSpeed = 100f;

    private void Update()
    {
        if (Input.GetKey(KeyCode.D))
            LeftRotate();

        if (Input.GetKey(KeyCode.A))
            RightRotate();
    }

    /// <summary>
    /// �E���i���v����j��]�������郁�\�b�h
    /// </summary>
    public void RightRotate()
    {
        this.transform.Rotate(_rotateSpeed * Time.deltaTime, 0, 0);
    }

    /// <summary>
    /// �����i�����v����j��]�������郁�\�b�h
    /// </summary>
    public void LeftRotate()
    {
        this.transform.Rotate(-(_rotateSpeed * Time.deltaTime), 0, 0);
    }
}
