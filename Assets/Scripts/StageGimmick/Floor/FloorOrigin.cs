using Alchemy.Inspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorOrigin : MonoBehaviour
{
    [LabelText("回転速度")]
    [SerializeField] private float _rotateSpeed = 100f;

    private void Update()
    {
        if (Input.GetKey(KeyCode.D))
            LeftRotate();

        if (Input.GetKey(KeyCode.A))
            RightRotate();
    }

    /// <summary>
    /// 右回り（時計周り）回転をさせるメソッド
    /// </summary>
    public void RightRotate()
    {
        this.transform.Rotate(_rotateSpeed * Time.deltaTime, 0, 0);
    }

    /// <summary>
    /// 左回り（反時計周り）回転をさせるメソッド
    /// </summary>
    public void LeftRotate()
    {
        this.transform.Rotate(-(_rotateSpeed * Time.deltaTime), 0, 0);
    }
}
