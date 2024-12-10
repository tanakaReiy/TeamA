using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObjectRotator : MonoBehaviour
{
    [SerializeField]
    private GameObject _rotatingObject;
    [SerializeField]
    private float _rotationAngle;
    private void Update()
    {
        if (Input.GetKey(KeyCode.N))
        {
            ApplyRotation();
        }
    }
    private void ApplyRotation()
    {
        _rotatingObject.transform.Rotate(0, _rotationAngle, 0);
    }
}
