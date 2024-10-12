using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    protected State _currentState = null;

    protected virtual void Update()
    {
        _currentState?.Tick(Time.deltaTime);
    }

    public void ChangeState(State nextState)
    {
        if(nextState == null) { throw new System.ArgumentNullException(); }
        _currentState?.Exit();
        _currentState = nextState;
        _currentState?.Enter();
    }
}
