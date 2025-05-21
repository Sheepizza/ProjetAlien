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
        Debug.Log("Entre dans l'état de Patrouille");
        if(alien.rooms.Count <= 0)
        alien.rooms.AddRange(GameObject.FindGameObjectsWithTag("Room"));

        alien.FindRoomManager();
    }

    public override void ExitState()
    {
        alien.inPatrol = false;    
    }

    public override void FrameUpdate()
    {
        if(alien.pathwayCountdown <= 0)
        {
            alien.inPatrol = false;
            alien.FindRoomManager();
        }

        if(alien.FOV.canSeePlayer)
        {
            stateMachine.ChangeState(alien.huntState);
        }
    }

    public override void PhysicsUpdate()
    {

    }
}
