using Alchemy.Inspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorOrigin : MonoBehaviour
{
    [LabelText("回転速度")]
    [SerializeField] private float _rotateSpeed = 100f;

    private List<RotateFloor> _rotateFloors = new List<RotateFloor>();

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
        if(CheckRight())
            this.transform.Rotate(_rotateSpeed * Time.deltaTime, 0, 0);
    }


    /// <summary>
    /// 左回り（反時計周り）回転をさせるメソッド
    /// </summary>
    public void LeftRotate()
    {
        if(CheckLeft())
            this.transform.Rotate(-(_rotateSpeed * Time.deltaTime), 0, 0);
    }

    public void RegisterFloor(RotateFloor floor)
    {
        _rotateFloors.Add(floor);
    }

    private bool CheckRight()
    {
        foreach (RotateFloor floor in _rotateFloors)
        {
            // オブジェクトの位置を取得
            Vector3 position = floor.transform.position;

            // オブジェクトの左側にRayを飛ばす
            RaycastHit hit;
            Vector3 rayDirection = floor.transform.forward;
            

            //// Rayを飛ばして衝突したオブジェクトをチェック
            //if (Physics.BoxCast(position, )
            //{
            //    // 衝突したオブジェクトが指定したタグを持っているか確認
            //    if (hit.collider.CompareTag("Detach"))
            //    {
            //        return false;
            //    }
            //}
        }

        // 何も見つからなければtrueを返す（または別の処理）
        return true;
    }

    private bool CheckLeft()
    {
        foreach(RotateFloor floor in _rotateFloors)
        {
            // オブジェクトの位置を取得
            Vector3 position = floor.transform.position;

            // オブジェクトの左側にRayを飛ばす
            RaycastHit hit;
            Vector3 rayDirection = -floor.transform.forward;

            // Rayを飛ばして衝突したオブジェクトをチェック
            if (Physics.Raycast(position, rayDirection, out hit))
            {
                // 衝突したオブジェクトが指定したタグを持っているか確認
                if (hit.collider.CompareTag("Detach"))
                {
                    return false;
                }
            }

            // デバッグ用にRayを表示 (Rayの始点と方向を指定)
            Debug.DrawRay(position, rayDirection * 10, Color.red); // Rayの長さは10に設定
        }

        // 何も見つからなければtrueを返す（または別の処理）
        return true;
    }
}
