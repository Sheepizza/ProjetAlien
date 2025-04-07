using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienState
{
    public AlienState(Alien _alien, AlienStateMachine _stateMachine)
    {
        _alien = alien;
        _stateMachine = stateMachine;
    }

    protected Alien alien;
    protected AlienStateMachine stateMachine;

    public virtual void EnterState() {}
    public virtual void ExitState() {}
    public virtual void FrameUpdate() {}
    public virtual void PhysicsUpdate() {}



}
