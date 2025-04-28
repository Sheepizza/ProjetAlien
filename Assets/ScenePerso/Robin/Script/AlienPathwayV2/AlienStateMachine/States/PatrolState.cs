using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : AlienState
{
    public PatrolState(Alien alien, AlienStateMachine stateMachine) : base(alien, stateMachine)
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entre dans l'état de Patrouille");
        alien.rooms.AddRange(GameObject.FindGameObjectsWithTag("Room"));

        alien.FindRoomManager();    
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if(alien.pathwayCountdown <= 0)
        {
        alien.FindRoomManager();
        }

        if(alien.FOV.canSeePlayer)
        {
            stateMachine.ChangeState(alien.huntState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
