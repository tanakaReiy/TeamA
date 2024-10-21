using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : SingletonMonoBehavior<InputReader>, Controls.IPlayerActions
{

    public IObservable<InputAction.CallbackContext> OnCaptureAsObservable() => _onCaptureSubject;
    public IObservable<InputAction.CallbackContext> OnSkillAsObservable() => _onSkillSubject;
    public IObservable<InputAction.CallbackContext> OnMoveAsObservable() => _onMoveSubject;
    public IObservable<InputAction.CallbackContext> OnLookAsObservable() => _onLookSubject;
    public IObservable<InputAction.CallbackContext> OnInteractAsObservable() => _onInteractSubject;

    public Vector2 MovementInput { get; private set; }


    private Subject<InputAction.CallbackContext> _onCaptureSubject = new();
    private Subject<InputAction.CallbackContext> _onSkillSubject = new();
    private Subject<InputAction.CallbackContext> _onMoveSubject = new();
    private Subject<InputAction.CallbackContext> _onLookSubject = new();
    private Subject<InputAction.CallbackContext> _onInteractSubject = new();

    private Controls controls;
    private void Start()
    {
        controls = new();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();

        OnMoveAsObservable()
            .Subscribe(context =>
            {
                MovementInput = context.ReadValue<Vector2>();
            }).AddTo(this);



    }

    public void OnCapture(InputAction.CallbackContext context)
    {
        _onCaptureSubject.OnNext(context);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _onLookSubject.OnNext(context);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _onMoveSubject.OnNext(context);
    }

    public void OnSkill(InputAction.CallbackContext context)
    {
        _onSkillSubject.OnNext(context); 
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        _onInteractSubject.OnNext(context);
    }
}
