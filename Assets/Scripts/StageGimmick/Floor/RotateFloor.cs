using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RotateFloor : MonoBehaviour
{
    Rigidbody _rb;
    bool _isSet;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
        {
            Debug.LogError("Rigidbody���A�^�b�`����Ă��܂���B");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RotateFloor"))
        {
            if (_isSet)
                return;
            _rb.isKinematic = false;
            this.transform.SetParent(other.transform);
            _isSet = true;
        }
        if (transform.parent != null && other.CompareTag("Detach"))
        {
            StopMove();
        }
    }

    private void StopMove()
    {
        this.transform.parent = null;
        _rb.isKinematic = true; // �����Ȃ��悤�ɂ���
        _isSet = false;
    }
}
