using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaladeState : AlienState
{
    public BaladeState(Alien alien, AlienStateMachine stateMachine) : base(alien, stateMachine)
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
        alien.StopCoroutine(alien.PathwayCountdown());
        alien.pathwayCountdownCoroutine = null;
    }

    public override void FrameUpdate()
    {
        alien.enemyNavMesh.SetDestination(alien.rooms[alien.actualRoom].transform.GetChild(Random.Range(0, alien.rooms[alien.actualRoom].transform.childCount)).position);
    }

    public override void PhysicsUpdate()
    {
        
    }
}
