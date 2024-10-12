using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Alchemy.Inspector;
public class PlayerStateMachine : StateMachine
{
    [field:SerializeField,FoldoutGroup("CompRefs")] public CharacterMovement CharacterMovement { get; private set; }


    private void Start()
    {
        ChangeState(new PlayerFreeLookState(this));

        Cursor.lockState = CursorLockMode.Locked;
    }
}
