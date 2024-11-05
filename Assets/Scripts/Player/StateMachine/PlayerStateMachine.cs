using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimaitonEventReceivable;
using Alchemy.Inspector;
using UniRx;

public class PlayerStateMachine : StateMachine,ICaptureAnimationEventReceivable
{
    [field: SerializeField, FoldoutGroup("CompRefs")] public CharacterMovement CharacterMovement { get; private set; }
    [field: SerializeField, FoldoutGroup("CompRefs")] public CharacterController Controller { get; private set; }
    [field: SerializeField, FoldoutGroup("CompRefs")] public InteractDetector InteractDetector { get; private set; }
    [field: SerializeField, FoldoutGroup("CompRefs")] public SocketManager SocketManager { get; private set; }
    [field: SerializeField, FoldoutGroup("CompRefs")] public Animator Animator { get; private set; }
    [field: SerializeField, FoldoutGroup("CompRefs")] public CapturableDetector CapturableDetector { get; private set; }
    [field: SerializeField, FoldoutGroup("CompRefs")] public PlayerStatus Status { get; private set; }
    [field: SerializeField, FoldoutGroup("CompRefs")] public WandManager WandManager { get; private set; }






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
    }

    #region AnimationEventReceiver
    public void EnableDetection()
    {
        if(_currentState is ICaptureAnimationEventReceivable state)
        {
            state.EnableDetection();
        }
    }

    public void DisableDetection()
    {
        if (_currentState is ICaptureAnimationEventReceivable state)
        {
            state.DisableDetection();
        }
    }
    #endregion
}
