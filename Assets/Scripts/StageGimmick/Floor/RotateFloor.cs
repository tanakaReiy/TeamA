using Alchemy.Inspector;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RotateFloor : MonoBehaviour
{
    [LabelText("登録できる回転軸")]
    [SerializeField] FloorOrigin _floorOrign;
    Rigidbody _rb;
    RotateFloor _rotateFloor;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rotateFloor = GetComponent<RotateFloor>();

        if (_rb == null)
        {
            Debug.LogError("Rigidbodyがアタッチされていません。");
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
