using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchingState : AlienState
{
    public SearchingState(Alien alien, AlienStateMachine stateMachine) : base(alien, stateMachine)
    {

    }

    public override void EnterState()
    {
        if (alien.pathwayCountdownCoroutine == null)
                {
                    alien.pathwayCountdownCoroutine = alien.StartCoroutine(alien.PathwayCountdown());
                }
    }

    public override void ExitState()
    {
        alien.pathwayCountdownCoroutine = null;
    }

    public override void FrameUpdate()
    {
       
    }

    public override void PhysicsUpdate()
    {
        
    }
}
