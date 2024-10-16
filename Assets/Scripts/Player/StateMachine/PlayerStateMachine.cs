using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Alchemy.Inspector;
public class PlayerStateMachine : StateMachine
{
    [field:SerializeField,FoldoutGroup("CompRefs")] public CharacterMovement CharacterMovement { get; private set; }
    [field: SerializeField, FoldoutGroup("CompRefs")] public InteractDetector InteractDetector { get; private set; }
    [field: SerializeField, FoldoutGroup("CompRefs")] public SocketManager SocketManager { get; private set; }


    [field: SerializeField] public float FreeLookMovementSpeed = 6f;
    [field: SerializeField] public float RotationDampTime = 5f;
    private void Start()
    {
        ChangeState(new PlayerFreeLookState(this));

        Cursor.lockState = CursorLockMode.Locked;
    }

    protected override void Update()
    {
        base.Update();
        SocketManager.GetSocket("StaffSocket").gameObject.transform.localRotation *= Quaternion.AngleAxis(30 * Time.deltaTime, Vector3.right);
    }
}
