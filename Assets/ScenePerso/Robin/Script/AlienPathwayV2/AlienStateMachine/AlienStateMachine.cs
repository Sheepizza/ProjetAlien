using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienStateMachine
{
    public AlienState _CurrentState {get; set;}

    public void Initialize(AlienState startingState)
    {
        _CurrentState = startingState;
        _CurrentState.EnterState();
    }

    public void ChangeState(AlienState newState)
    {
        _CurrentState.ExitState();
        _CurrentState = newState;
        _CurrentState.EnterState();
    }
}
