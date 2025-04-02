using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : AlienState
{
    public PatrolState(Alien alien, AlienStateMachine stateMachine) : base (alien, stateMachine)
    {

    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
