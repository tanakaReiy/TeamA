using Alchemy.Inspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorOrigin : MonoBehaviour
{
    [LabelText("��]���x")]
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
    /// �E���i���v����j��]�������郁�\�b�h
    /// </summary>
    public void RightRotate()
    {
        if(CheckRight())
            this.transform.Rotate(_rotateSpeed * Time.deltaTime, 0, 0);
    }


    /// <summary>
    /// �����i�����v����j��]�������郁�\�b�h
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
            // �I�u�W�F�N�g�̈ʒu���擾
            Vector3 position = floor.transform.position;

            // �I�u�W�F�N�g�̍�����Ray���΂�
            RaycastHit hit;
            Vector3 rayDirection = floor.transform.forward;
            

            //// Ray���΂��ďՓ˂����I�u�W�F�N�g���`�F�b�N
            //if (Physics.BoxCast(position, )
            //{
            //    // �Փ˂����I�u�W�F�N�g���w�肵���^�O�������Ă��邩�m�F
            //    if (hit.collider.CompareTag("Detach"))
            //    {
            //        return false;
            //    }
            //}
        }

        // ����������Ȃ����true��Ԃ��i�܂��͕ʂ̏����j
        return true;
    }

    private bool CheckLeft()
    {
        foreach(RotateFloor floor in _rotateFloors)
        {
            // �I�u�W�F�N�g�̈ʒu���擾
            Vector3 position = floor.transform.position;

            // �I�u�W�F�N�g�̍�����Ray���΂�
            RaycastHit hit;
            Vector3 rayDirection = -floor.transform.forward;

            // Ray���΂��ďՓ˂����I�u�W�F�N�g���`�F�b�N
            if (Physics.Raycast(position, rayDirection, out hit))
            {
                // �Փ˂����I�u�W�F�N�g���w�肵���^�O�������Ă��邩�m�F
                if (hit.collider.CompareTag("Detach"))
                {
                    return false;
                }
            }

            // �f�o�b�O�p��Ray��\�� (Ray�̎n�_�ƕ������w��)
            Debug.DrawRay(position, rayDirection * 10, Color.red); // Ray�̒�����10�ɐݒ�
        }

        // ����������Ȃ����true��Ԃ��i�܂��͕ʂ̏����j
        return true;
    }
}
