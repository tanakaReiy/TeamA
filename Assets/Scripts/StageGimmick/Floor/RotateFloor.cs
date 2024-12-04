using Alchemy.Inspector;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RotateFloor : MonoBehaviour
{
    [LabelText("�o�^�ł����]��")]
    [SerializeField] FloorOrigin _floorOrign;
    Rigidbody _rb;
    RotateFloor _rotateFloor;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rotateFloor = GetComponent<RotateFloor>();

        if (_rb == null)
        {
            Debug.LogError("Rigidbody���A�^�b�`����Ă��܂���B");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RotateFloor") && transform.parent == null)
        {
            _rb.isKinematic = false;
            this.transform.SetParent(other.transform);
            _floorOrign.RegisterFloor(_rotateFloor);
        }
    }
}
