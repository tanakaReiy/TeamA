using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimaitonEventReceivable;
using Alchemy.Inspector;
using UniRx;

public class PlayerStateMachine : StateMachine, IPlayerAnimationSePlayable
{

    [field: SerializeField, FoldoutGroup("CompRefs")] public CharacterMovement CharacterMovement { get; private set; }
    [field: SerializeField, FoldoutGroup("CompRefs")] public CharacterController Controller { get; private set; }
    [field: SerializeField, FoldoutGroup("CompRefs")] public InteractDetector InteractDetector { get; private set; }
    [field: SerializeField, FoldoutGroup("CompRefs")] public SocketManager SocketManager { get; private set; }
    [field: SerializeField, FoldoutGroup("CompRefs")] public Animator Animator { get; private set; }
    [field: SerializeField, FoldoutGroup("CompRefs")] public Capturaing Capturaing { get; private set; }
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


    public void PlaySe(string cueName)
    {
        CRIAudioManager.SE.Play3D(Vector3.zero, PlayerBaseState.CueSheetName, cueName);
    }

    public void PlaySeThroughState(string cueName)
    {
        if(_currentState is IPlayerAnimationSePlayable sePlayable)
        {
            sePlayable.PlaySeThroughState(cueName);
        }
    }
    #endregion
}

namespace AnimaitonEventReceivable
{
    public interface IPlayerAnimationSePlayable
    {
        void PlaySe(string cueName);

        void PlaySeThroughState(string cueName);
    }
}